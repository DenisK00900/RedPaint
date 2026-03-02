using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionFileBrowser : Action
    {
        public DialogFileLoad fileLoad;

        public string text;

        public override void Act()
        {
            fileLoad.UpdateListInfo(text);
        }

        public ActionFileBrowser(Maincode imc, DialogFileLoad fl, string tx) : base(imc)
        {
            fileLoad = fl;
            text = tx;
        }
    }
}
