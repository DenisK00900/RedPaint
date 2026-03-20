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

        public List<Layer> layers = new List<Layer>();
        public int workingLayer = 0;

        private Texture2D canvasImage = null;

        private Color[] pixelBuffer = null;
        private bool isDirty = false;

        private Rectangle dirtyRect = Rectangle.Empty;

        public event Action ImageLoaded;
        public event Action ChangesApplied;
        public event Action ChangesLayers;

        public Vector2 CanvasSize;
        public CheckerTex checkerTex;

        public event Action ToolChanged;

        private AbstrTool _currTool;
        public AbstrTool currTool
        {
            get => _currTool;
            set
            {
                if (_currTool != value)
                {
                    _currTool = value;
                    ToolChanged?.Invoke();
                }
            }
        }

        private Color paintColor = Color.Red;

        public bool isModified = false;

        public ImageManager(Maincode imc)
        {
            mc = imc;
            checkerTex = new CheckerTex(mc);
        }

        public void AddLayer(Layer lr = null)
        {
            if (lr != null)
            {
                layers.Add(lr);

                ChangesLayers.Invoke();

                return;
            }

            AddBlankLayer();
        }

        public void AddBlankLayer()
        {
            var newLayer = new Layer(mc);
            newLayer.tex = new Texture2D(mc.GraphicsDevice, canvasImage.Width, canvasImage.Height);

            Color[] transparentPixels = new Color[canvasImage.Width * canvasImage.Height];
            for (int i = 0; i < transparentPixels.Length; i++)
                transparentPixels[i] = Color.Transparent;
            newLayer.tex.SetData(transparentPixels);

            layers.Add(newLayer);
            ChangesLayers.Invoke();
        }

        public void RemoveLayer(int index)
        {
            layers.RemoveAt(index);

            SetWorkingLayer(0);

            ChangesLayers.Invoke();
        }

        public void SetColor(Color newcolor)
        {
            paintColor.R = ((byte)newcolor.R);
            paintColor.G = ((byte)newcolor.G);
            paintColor.B = ((byte)newcolor.B);
        }

        public void SetAlpha(float newalpha)
        {
            paintColor.A = ((byte)(newalpha*255));
        }

        public Color GetColor()
        {
            return paintColor;
        }

        public void SetImage(Texture2D tex, int layerIndex = 0)
        {
            layers[layerIndex].tex = tex;
            InitPixelBuffer();
            UpdateCanvas();
        }

        public void CreateNew(Texture2D t)
        {
            AddLayer(new Layer(mc));
            layers[0].tex = t;
            InitPixelBuffer();
            UpdateCanvas();
            ImageLoaded?.Invoke();

            isModified = false;
        }

        public void LoadImage(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Файл изображения не найден: {path}");

            layers.Clear();

            try
            {
                AddLayer(new Layer(mc));
                using (var stream = File.OpenRead(path))
                {
                    layers[0].tex = Texture2D.FromStream(mc.GraphicsDevice, stream);
                }
                InitPixelBuffer();
                CanvasSize = TUH.GetTextureSize(layers[0].tex);
                UpdateCanvas();
                ImageLoaded?.Invoke();

                isModified = false;
            }
            catch (Exception ex)
            {
                mc._entityManager.AddEntity(
                    new DialogError(mc, "Ошибка загрузки изображения",
                    FileBrowserSolver.TrimExceptionMessage(ex), null));
            }
        }

        public bool CanWrite()
        {
            return layers.Count > 0 && !layers[workingLayer].isLocked;
        }

        private void InitPixelBuffer()
        {
            if (layers[workingLayer].tex == null) return;

            pixelBuffer = new Color[layers[workingLayer].tex.Width * layers[workingLayer].tex.Height];
            layers[workingLayer].tex.GetData(pixelBuffer);
            isDirty = false;
            dirtyRect = Rectangle.Empty;
        }

        public bool SetPixel(int x, int y, Color color)
        {
            if (layers[workingLayer].tex == null || pixelBuffer == null) return false;
            if (x < 0 || x >= layers[workingLayer].tex.Width || y < 0 || y >= layers[workingLayer].tex.Height) return false;

            int index = y * layers[workingLayer].tex.Width + x;

            if (pixelBuffer[index] == color) return true;

            pixelBuffer[index] = color;
            MarkDirty(x, y);

            isModified = true;

            return true;
        }

        public Color GetPixel(int x, int y)
        {
            if (layers[workingLayer].tex == null || pixelBuffer == null) return Color.Transparent;
            if (x < 0 || x >= layers[workingLayer].tex.Width || y < 0 || y >= layers[workingLayer].tex.Height) return Color.Transparent;

            return pixelBuffer[y * layers[workingLayer].tex.Width + x];
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
            if (!isDirty || layers[workingLayer].tex == null || pixelBuffer == null) return;

            layers[workingLayer].tex.SetData(pixelBuffer);

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
                    result[dy * rect.Width + dx] = pixelBuffer[srcY * layers[workingLayer].tex.Width + srcX];
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

        public Texture2D GetCurrentImage() => layers[workingLayer].tex;
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
            return new Vector2(-1f, -1f);
        }

        public void SetWorkingLayer(int index)
        {
            if (index < 0 || index >= layers.Count) return;

            if (isDirty) ApplyChanges();

            workingLayer = index;
            InitPixelBuffer();
            ChangesLayers?.Invoke();
        }

        public void UpdateCanvas()
        {
            if (layers[workingLayer].tex == null) return;

            //checkerTex.Dispose();

            checkerTex.sizeX = layers[workingLayer].tex.Width;
            checkerTex.sizeY = layers[workingLayer].tex.Height;
            checkerTex.sizeChecker = 16;
            checkerTex.color1 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor1, 0.9f);
            checkerTex.color2 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor2, 0.9f);
            checkerTex.Generate();
            canvasImage = checkerTex.Tex;
        }

        public void Apply()
        {
            foreach (Panel pl in mc.mainHolder.panels)
            {
                if (pl.panelActive is ImageView iv)
                {
                    if (pl.GetActiveRect().CheckPoint(mc._input.GetMousePosition()))
                    {
                        ApplyChanges();
                        return;
                    }
                }
            }
        }
    }
}