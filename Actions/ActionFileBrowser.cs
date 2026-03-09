using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionFileBrowser : AbstrAction
    {
        public IFileBrowser fileLoad;

        public string text;

        public override void Act()
        {
            Exception ex = FileBrowserSolver.CanOpenPathEx(fileLoad.currDir + text);

            if (ex == null)
            {
                fileLoad.UpdateListInfo(text);
            }
            else
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка браузера файлов",
                        FileBrowserSolver.TrimExceptionMessage(ex),
                        null
                        ));
            }
        }

        public ActionFileBrowser(Maincode imc, IFileBrowser fl, string tx) : base(imc)
        {
            fileLoad = fl;
            text = tx;
        }
    }
}
