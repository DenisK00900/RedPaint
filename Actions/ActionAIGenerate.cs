using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RedPaint
{
    public class ActionAIGenerate : AbstrAction
    {
        public string prompt;
        public string negativePrompt;

        public Vector2 size = new Vector2(1024, 1024);

        private AiImageGenerator _aiGenerator;
        private Task<Texture2D> _generationTask;
        private CancellationTokenSource _cts;
        private bool _isGenerating = false;

        public ActionAIGenerate(Maincode imc) : base(imc)
        {

        }

        public override void Act()
        {
            string apiKey = mc._settings.GetAPIkey();

            if (apiKey.Length == 0)
            {
                mc._entityManager.AddEntity(
                    new DialogError(
                        mc,
                        "Ошибка ИИ генерации",
                        "Отсутсвует ключ. Введите ключ или используйте встроенный.",
                        null
                        ));

                return;
            }

            prompt = mc._data.promnt;
            negativePrompt = mc._data.negativePromnt;

            if (prompt.Length == 0)
            {
                mc._entityManager.AddEntity(
                    new DialogError(
                        mc,
                        "Ошибка ИИ генерации",
                        "Отсутсвует запрос. Редактируйте запрос перед началом генерации",
                        null
                        ));

                return;
            }

            if (!mc._image.IsCreated())
            {
                mc._entityManager.AddEntity(
                    new DialogError(
                        mc,
                        "Ошибка ИИ генерации",
                        "Нет изображения. Создайте изображение перед началом генерации",
                        null
                        ));

                return;
            }

            _aiGenerator = new AiImageGenerator(apiKey, mc)
            {
                DebugMode = true,
            };

            if (_isGenerating) return;

            if (mc._image?.GetCanvas() != null)
            {
                size = TUH.GetTextureSize(mc._image.GetCanvas());
            }

            _isGenerating = true;
            _cts = new CancellationTokenSource();
            _generationTask = GenerateAndApplyAsync(_cts.Token);
        }

        private int RandomSeed()
        {
            Random random = new Random();

            return random.Next(0,999999);
        }

        private async Task<Texture2D> GenerateAndApplyAsync(CancellationToken ct)
        {
            try
            {
                var texture = await _aiGenerator.GenerateSDXLAsync(
                    mc.GraphicsDevice,
                    mc._data.promnt,
                    mc._data.negativePromnt,
                    (int)size.X,
                    (int)size.Y,
                    mc._data.AIsteps,
                    mc._data.AIscale,
                    mc._data.AIseed < 0 ? RandomSeed() : mc._data.AIseed,
                    ct);

                ApplyToCanvas(texture);

                return texture;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                mc._entityManager?.AddEntity(
                    new DialogError(mc, "Ошибка SDXL",
                    $"{ex.GetType().Name}:\n{ex.Message}\n\n" +
                    $"Промт: {prompt}\n" +
                    $"Размер: {size.X}x{size.Y}\n\n" +
                    $"Проверьте:\n" +
                    $"• API ключ Pixazo\n" +
                    $"• Доступен ли баланс\n" +
                    $"• Корректность промта",
                    null));

                return null;
            }
            finally
            {
                _isGenerating = false;
            }
        }

        private void ApplyToCanvas(Texture2D newTexture)
        {
            if (newTexture == null || mc._image == null) return;

            var newLayer = new Layer(mc);
            newLayer.tex = newTexture;
            newLayer.name = "ИИ генерация";
            mc._image.AddLayer(newLayer);
            mc._image.SetWorkingLayer(mc._image.layers.Count - 1);

            mc._image.InitPixelBuffer();
            mc._image.CallChanges();
            mc._image.isModified = true;
        }

        public void Update()
        {
            if (_generationTask?.IsCompleted == true) _generationTask = null;
            if (_generationTask?.IsFaulted == true)
            {
                _generationTask = null;
            }
        }

        public void Cancel()
        {
            if (_isGenerating && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }
    }
}