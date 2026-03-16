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

        public bool force = false;

        public override void Act()
        {
            if (!force)
            {
                if (!(size.X > 0 && size.Y > 0))
                {
                    mc._entityManager.AddEntity(
                    new DialogError(
                                mc,
                                "Ошибка создания изображения",
                                FileBrowserSolver.TrimExceptionMessage(new Exception("Размер должен быть больше 0")),
                                null
                                ));

                    return;
                }

                if (mc._image.isModified)
                {
                    DialogWarning dw = new DialogWarning(
                                mc,
                                "Ошибка создания изображения",
                                "Текущий файл был изменён и не был сохранён. Создание нового файла перезапишет его и\nизменения будут потерены!",
                                null);

                    dw.SetAgreeText("Создать новый");

                    ActionNewImage forseNew = new ActionNewImage(mc);
                    forseNew.force = true;
                    forseNew.size = size;

                    dw.agree.AddAction(forseNew);
                    dw.agree.AddAction(new ActionDestroy(mc, dw));

                    dw.agree.hint = new Hint(mc, "Создать новый файл.\nСодержимое текущего файла будет уничтожено");

                    dw.SetDisagreeText("Отмена");

                    dw.disagree.AddAction(new ActionDestroy(mc, dw));

                    dw.disagree.hint = new Hint(mc, "Отменить действие и не создавать новый файл.");

                    mc._entityManager.AddEntity(dw);

                    return;
                }
            }

            mc._image.CreateNew(TUH.CreateTransparentTexture(mc.GraphicsDevice, (int)Math.Round(size.X), (int)Math.Round(size.Y)));
        }

        public ActionNewImage(Maincode imc) : base(imc)
        {

        }
    }
}
