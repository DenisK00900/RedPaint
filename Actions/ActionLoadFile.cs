using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RedPaint
{
    public class ActionLoadFile : Action
    {
        public string fullPath;
        public DialogFileLoad fileLoad;

        public override void Act()
        {
            Debug.WriteLine($"Загрузка : {fullPath}");

            fileLoad.Destroy();
        }

        public ActionLoadFile(Maincode imc, DialogFileLoad fl, string fp) : base(imc)
        {
            fullPath = fp;
            fileLoad = fl;
        }
    }
}
