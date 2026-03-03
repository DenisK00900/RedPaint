using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RedPaint
{
    public class ActionLoadFile : AbstrAction
    {
        public string fullPath;
        public DialogFileLoad fileLoad;

        public override void Act()
        {
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
