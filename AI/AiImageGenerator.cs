using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = SixLabors.ImageSharp.Image;

namespace RedPaint
{
    public class AiImageGenerator : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string SDXL_Url = "https://gateway.pixazo.ai/getImage/v1/getSDXLImage";

        public bool DebugMode { get; set; } = true;
        public Action<string> OnLog { get; set; }

        private readonly Random _random;

        Maincode mc;

        public AiImageGenerator(string apiKey, Maincode imc)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;

            mc = imc;

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.Timeout = TimeSpan.FromMinutes(3);

            _random = new Random();
        }

        public async Task<Texture2D> GenerateSDXLAsync(
            GraphicsDevice graphicsDevice,
            string prompt,
            string negativePrompt = "",
            int width = 1024,
            int height = 1024,
            int numSteps = 20,
            float guidanceScale = 5.0f,
            int? seed = null,
            CancellationToken ct = default)
        {
            try
            {
                mc._status.SetNoFade(true);

                mc._status.SetLog("Получен запрос...");

                Log($"[Pixazo SDXL] Запрос: {prompt}");
                Log($"[Pixazo SDXL] Целевой размер: {width}x{height}");

                var requestBody = new
                {
                    prompt = prompt,
                    negative_prompt = negativePrompt,
                    height = 1024,
                    width = 1024,
                    num_steps = numSteps,
                    guidance_scale = guidanceScale,
                    seed = seed ?? _random.Next(0, 999999)
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                mc._status.SetLog("Запрос отправлен на сервер...");

                _httpClient.DefaultRequestHeaders.Remove("Cache-Control");
                _httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                Log($"[Pixazo SDXL] POST {SDXL_Url}");
                Log($"[Pixazo SDXL] Body: {Truncate(json, 500)}");

                mc._status.SetLog("Ожидание ответа сервера...");

                var response = await _httpClient.PostAsync(SDXL_Url, content, ct);
                var responseText = await response.Content.ReadAsStringAsync(ct);

                Log($"[Pixazo SDXL] Статус: {response.StatusCode}");
                Log($"[Pixazo SDXL] Response: {Truncate(responseText, 1000)}");

                mc._status.SetLog("Ответ получен...");

                if (!response.IsSuccessStatusCode)
                {
                    mc._status.SetLog("Ошибка!");
                    mc._status.SetNoFade(false);

                    mc._entityManager.AddEntity(
                    new DialogError(
                                mc,
                                "Ошибка ИИ генерации",
                                $"Pixazo SDXL API error {response.StatusCode}: {responseText}",
                                null
                                ));
                }

                using var doc = JsonDocument.Parse(responseText);
                var root = doc.RootElement;

                string base64Image = null;
                string imageUrl = null;

                if (root.TryGetProperty("imageUrl", out var imgUrlProp))
                    imageUrl = imgUrlProp.GetString();
                else if (root.TryGetProperty("image_url", out var imgUrlProp2))
                    imageUrl = imgUrlProp2.GetString();
                else if (root.TryGetProperty("image", out var imgProp))
                {
                    var imgStr = imgProp.GetString();
                    if (imgStr?.StartsWith("http") == true)
                        imageUrl = imgStr;
                    else
                        base64Image = imgStr;
                }
                else if (root.TryGetProperty("images", out var arr) && arr.GetArrayLength() > 0)
                {
                    var imgStr = arr[0].GetString();
                    if (imgStr?.StartsWith("http") == true)
                        imageUrl = imgStr;
                    else
                        base64Image = imgStr;
                }
                else if (root.TryGetProperty("output", out var output))
                {
                    if (output.TryGetProperty("imageUrl", out var outUrl))
                        imageUrl = outUrl.GetString();
                    else if (output.TryGetProperty("image", out var outImg))
                    {
                        var imgStr = outImg.GetString();
                        if (imgStr?.StartsWith("http") == true)
                            imageUrl = imgStr;
                        else
                            base64Image = imgStr;
                    }
                    else if (output.TryGetProperty("media_url", out var media) && media.GetArrayLength() > 0)
                        imageUrl = media[0].GetString();
                }

                if (string.IsNullOrEmpty(imageUrl) && string.IsNullOrEmpty(base64Image))
                {
                    mc._status.SetLog("Ошибка");
                    mc._status.SetNoFade(false);

                    var availableProps = GetAvailableProperties(root);
                    throw new InvalidOperationException(
                        $"Не удалось найти изображение в ответе.\n" +
                        $"Доступные поля: {availableProps}\n" +
                        $"Полный ответ: {responseText}");
                }

                mc._status.SetLog("Скачивание изображения...");

                byte[] imageBytes;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    using var headResponse = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, ct);
                    var contentLength = headResponse.Content.Headers.ContentLength;

                    if (contentLength.HasValue)
                    {
                        var sizeMb = contentLength.Value / (1024f * 1024f);
                        mc._status.SetLog($"Скачивание изображения... ({sizeMb:F2} МБ)");
                        Log($"[Pixazo SDXL] Скачивание: {imageUrl} | Размер: {sizeMb:F2} МБ");
                    }
                    else
                    {
                        mc._status.SetLog("Скачивание изображения... (размер неизвестен)");
                        Log($"[Pixazo SDXL] Скачивание: {imageUrl}");
                    }

                    // Скачиваем с отслеживанием прогресса
                    var progress = new Progress<float>(percent =>
                    {
                        if (contentLength.HasValue)
                        {
                            var downloadedMb = (contentLength.Value * percent) / (1024f * 1024f);
                            var totalMb = contentLength.Value / (1024f * 1024f);
                            mc._status.SetLog($"Загрузка: {downloadedMb:F1}/{totalMb:F1} МБ ({percent * 100:F0}%)");
                        }
                    });

                    imageBytes = await DownloadWithProgressAsync(imageUrl, ct, progress);
                }
                else
                {
                    if (base64Image.Contains(","))
                        base64Image = base64Image.Split(',')[1];

                    Log($"[Pixazo SDXL] Декодирование base64 ({base64Image.Length} символов)");
                    imageBytes = Convert.FromBase64String(base64Image);
                }

                Log($"[Pixazo SDXL] Получено {imageBytes.Length} байт");

                if (width != 1024 || height != 1024)
                {
                    Log($"[Pixazo SDXL] Масштабирование до {width}x{height}");
                    imageBytes = ResizeImageBytes(imageBytes, width, height);
                    Log($"[Pixazo SDXL] После масштабирования: {imageBytes.Length} байт");
                }

                using var ms = new MemoryStream(imageBytes);

                mc._status.SetLog("Готово!");
                mc._status.SetNoFade(false);

                return Texture2D.FromStream(graphicsDevice, ms);
            }
            catch (Exception ex)
            {
                mc._status.SetLog("Ошибка!");
                mc._status.SetNoFade(false);

                Log($"[Pixazo SDXL] Ошибка: {ex.Message}");
                throw;
            }
        }

        private async Task<byte[]> DownloadWithProgressAsync(string url, CancellationToken ct, IProgress<float> progress = null)
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var ms = new MemoryStream();

            var buffer = new byte[8192];
            long totalRead = 0;
            var totalBytes = response.Content.Headers.ContentLength;

            int read;
            while ((read = await stream.ReadAsync(buffer, ct)) > 0)
            {
                await ms.WriteAsync(buffer.AsMemory(0, read), ct);
                totalRead += read;

                if (totalBytes.HasValue && progress != null)
                {
                    var percent = (float)totalRead / totalBytes.Value;
                    progress?.Report(percent);
                }
            }

            return ms.ToArray();
        }

        private byte[] ResizeImageBytes(byte[] imageBytes, int targetWidth, int targetHeight)
        {
            using var inputStream = new MemoryStream(imageBytes);
            using var image = Image.Load(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(targetWidth, targetHeight),
                Mode = ResizeMode.Stretch,
                Sampler = KnownResamplers.Lanczos3
            }));

            using var outputStream = new MemoryStream();

            image.SaveAsPng(outputStream);
            return outputStream.ToArray();
        }

        private string GetAvailableProperties(JsonElement element)
        {
            var props = new List<string>();
            foreach (var prop in element.EnumerateObject())
                props.Add(prop.Name);
            return string.Join(", ", props);
        }

        private void Log(string message)
        {
            if (DebugMode)
            {
                System.Diagnostics.Debug.WriteLine(message);
                OnLog?.Invoke(message);
                Console.WriteLine($"[Pixazo] {message}");
            }
        }

        private string Truncate(string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLen) return text;
            return text.Substring(0, maxLen) + "...";
        }

        public void Dispose() => _httpClient?.Dispose();
    }
}