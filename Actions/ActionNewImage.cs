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
    public class ActionNewImage : AbstrAction
    {
        public Vector2 size;

        public override void Act()
        {

            if (!(size.X > 0 && size.Y > 0))
            {
                mc._entityManager.AddEntity(
                new DialogMessage(
                            mc,
                            "Ошибка создания изображения",
                            FileBrowserSolver.TrimExceptionMessage(new Exception("Размер должен быть больше 0")),
                            null
                            ));

                return;
            }

            mc._image.CreateNew(TUH.CreateTransparentTexture(mc.GraphicsDevice, (int)Math.Round(size.X), (int)Math.Round(size.Y)));
        }

        public ActionNewImage(Maincode imc) : base(imc)
        {

        }
    }
}
