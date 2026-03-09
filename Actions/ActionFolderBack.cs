using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionFolderBack : AbstrAction
    {
        public IFileBrowser fileLoad;

        public override void Act()
        {
            fileLoad.FolderUp();
        }

        public ActionFolderBack(Maincode imc, IFileBrowser fl) : base(imc)
        {
            fileLoad = fl;
        }
    }
}
