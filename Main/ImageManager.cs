using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedPaint
{
    public class ImageManager
    {
        Maincode mc;
        private Texture2D currImage = null;

        public event Action ImageLoaded;

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