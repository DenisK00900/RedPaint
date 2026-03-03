using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionFolderBack : AbstrAction
    {
        public DialogFileLoad fileLoad;

        public override void Act()
        {
            fileLoad.FolderUp();
        }

        public ActionFolderBack(Maincode imc, DialogFileLoad fl) : base(imc)
        {
            fileLoad = fl;
        }
    }
}
