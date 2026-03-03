using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using RedPaint;
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

        public event Action ImageLoaded;

        public Vector2 CanvasSize;

        public ImageManager(Maincode imc)
        {
            mc = imc;
        }

        public void LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Файл изображения не найден: {path}");
            }

            if (currImage != null)
            {
                currImage.Dispose();
                currImage = null;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    currImage = Texture2D.FromStream(mc.GraphicsDevice, stream);
                }

                CanvasSize = TUH.GetTextureSize(currImage);

                ImageLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке изображения: {ex.Message}", ex);
            }
        }

        public Texture2D GetCurrentImage()
        {
            return currImage;
        }
    }
}