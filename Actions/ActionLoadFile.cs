using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace RedPaint
{
    public class ActionLoadFile : AbstrAction
    {
        public string fullPath;
        public DialogFileLoad fileLoad;

        bool forse = false;

        public override void Act()
        {
            if (!forse)
            {
                if (mc._image.isModified)
                {
                    DialogWarning dw = new DialogWarning(
                                mc,
                                "Ошибка загрузки изображения",
                                "Текущий файл был изменён и не был сохранён. Загрузка нового файла перезапишет его и\nизменения будут потерены!",
                                null);

                    dw.SetAgreeText("Загрузить");

                    ActionLoadFile forseLoad = new ActionLoadFile(mc, fileLoad, fullPath);
                    forseLoad.forse = true;

                    dw.agree.AddAction(forseLoad);
                    dw.agree.AddAction(new ActionDestroy(mc, dw));

                    dw.agree.hint = new Hint(mc, "Загрузить новый файл.\nСодержимое текущего файла будет уничтожено");

                    dw.SetDisagreeText("Отмена");

                    dw.disagree.AddAction(new ActionDestroy(mc, dw));

                    dw.disagree.hint = new Hint(mc, "Отменить действие и не загружать новый файл.");

                    mc._entityManager.AddEntity(dw);

                    return;
                }
            }

            fileLoad.mc._image.LoadImage(fullPath);

            fileLoad.Destroy();
        }

        public ActionLoadFile(Maincode imc, DialogFileLoad fl, string fp) : base(imc)
        {
            fullPath = fp;
            fileLoad = fl;
        }
    }
}
