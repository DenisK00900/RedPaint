using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ImageManager
    {
        Maincode mc;
        private Texture2D currImage = null;
        private Texture2D canvasImage = null;

        private Color[] pixelBuffer = null;
        private bool isDirty = false;

        private Rectangle dirtyRect = Rectangle.Empty;

        public event Action ImageLoaded;
        public event Action ChangesApplied;

        public Vector2 CanvasSize;
        public CheckerTex checkerTex;
        public AbstrTool currTool = null;

        public ImageManager(Maincode imc)
        {
            mc = imc;
            checkerTex = new CheckerTex(mc);
        }
        public void SetImage(Texture2D tex)
        {
            currImage = tex;
            InitPixelBuffer();
            UpdateCanvas();
        }

        public void CreateNew(Texture2D t)
        {
            currImage = t;
            InitPixelBuffer();
            UpdateCanvas();
            ImageLoaded?.Invoke();
        }

        public void LoadImage(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл изображения не найден: {path}");

            currImage?.Dispose();

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    currImage = Texture2D.FromStream(mc.GraphicsDevice, stream);
                }
                InitPixelBuffer();
                CanvasSize = TUH.GetTextureSize(currImage);
                UpdateCanvas();
                ImageLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(mc, "Ошибка загрузки изображения",
                    FileBrowserSolver.TrimExceptionMessage(ex), null));
            }
        }

        private void InitPixelBuffer()
        {
            if (currImage == null) return;

            pixelBuffer = new Color[currImage.Width * currImage.Height];
            currImage.GetData(pixelBuffer);
            isDirty = false;
            dirtyRect = Rectangle.Empty;
        }

        public bool SetPixel(int x, int y, Color color)
        {
            if (currImage == null || pixelBuffer == null) return false;
            if (x < 0 || x >= currImage.Width || y < 0 || y >= currImage.Height) return false;

            int index = y * currImage.Width + x;

            if (pixelBuffer[index] == color) return true;

            pixelBuffer[index] = color;
            MarkDirty(x, y);
            return true;
        }

        public Color GetPixel(int x, int y)
        {
            if (currImage == null || pixelBuffer == null) return Color.Transparent;
            if (x < 0 || x >= currImage.Width || y < 0 || y >= currImage.Height) return Color.Transparent;

            return pixelBuffer[y * currImage.Width + x];
        }

        private void MarkDirty(int x, int y)
        {
            isDirty = true;

            if (dirtyRect.IsEmpty)
                dirtyRect = new Rectangle(x, y, 1, 1);
            else
            {
                dirtyRect = Rectangle.Union(dirtyRect, new Rectangle(x, y, 1, 1));
            }
        }
        public void ApplyChanges()
        {
            if (!isDirty || currImage == null || pixelBuffer == null) return;

            currImage.SetData(pixelBuffer);

            isDirty = false;
            dirtyRect = Rectangle.Empty;
            ChangesApplied?.Invoke();
        }
        private Color[] GetSubRectData(Rectangle rect)
        {
            Color[] result = new Color[rect.Width * rect.Height];
            for (int dy = 0; dy < rect.Height; dy++)
            {
                for (int dx = 0; dx < rect.Width; dx++)
                {
                    int srcX = rect.X + dx;
                    int srcY = rect.Y + dy;
                    result[dy * rect.Width + dx] = pixelBuffer[srcY * currImage.Width + srcX];
                }
            }
            return result;
        }

        public void ClearBuffer()
        {
            pixelBuffer = null;
            isDirty = false;
            dirtyRect = Rectangle.Empty;
        }

        public Texture2D GetCurrentImage() => currImage;
        public Texture2D GetCanvas() => canvasImage;
        public Color[] GetPixelBuffer() => pixelBuffer;
        public bool HasUnappliedChanges() => isDirty;

        public Vector2 GetTexPos()
        {
            foreach (Panel pl in mc.mainHolder.panels)
            {
                if (pl.panelActive is ImageView iv)
                    return iv.GetCurrTexPos();
            }
            return Vector2.Zero;
        }

        public void UpdateCanvas()
        {
            if (currImage == null) return;

            checkerTex.sizeX = currImage.Width;
            checkerTex.sizeY = currImage.Height;
            checkerTex.sizeChecker = 16;
            checkerTex.color1 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor1, 0.9f);
            checkerTex.color2 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor2, 0.9f);
            checkerTex.Generate();
            canvasImage = checkerTex.Tex;
        }

        public void Apply() => ApplyChanges();
    }
}