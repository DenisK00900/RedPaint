using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace RedPaint
{
    public class ActionAIGenerate : AbstrAction
    {
        public string prompt = "An apple on the table outside";
        public string negativePrompt = "Low-quality, blurry image, abstract, cartoonish, dark atmosphere, harsh lighting";
        public Vector2 size = new Vector2(1024, 1024);

        public int numSteps = 20;
        public float guidanceScale = 5.0f;
        public int? seed = null;

        private AiImageGenerator _aiGenerator;
        private Task<Texture2D> _generationTask;
        private CancellationTokenSource _cts;
        private bool _isGenerating = false;

        public ActionAIGenerate(Maincode imc) : base(imc)
        {
            var apiKey = "bf1e9e0c1c2246a78a66170449b70bb9";

            _aiGenerator = new AiImageGenerator(apiKey, mc)
            {
                DebugMode = false,
            };
        }

        public override void Act()
        {
            if (_isGenerating) return;

            if (mc._image?.GetCanvas() != null)
            {
                size = TUH.GetTextureSize(mc._image.GetCanvas());
            }

            _isGenerating = true;
            _cts = new CancellationTokenSource();
            _generationTask = GenerateAndApplyAsync(_cts.Token);
        }

        private async Task<Texture2D> GenerateAndApplyAsync(CancellationToken ct)
        {
            try
            {
                var texture = await _aiGenerator.GenerateSDXLAsync(
                    mc.GraphicsDevice,
                    prompt,
                    negativePrompt,
                    (int)size.X,
                    (int)size.Y,
                    numSteps,
                    guidanceScale,
                    seed,
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