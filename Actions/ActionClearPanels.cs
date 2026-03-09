using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionClearPanels : AbstrAction
    {
        public override void Act()
        {
            mc.mainHolder.ClearPanels();
        }
        public ActionClearPanels(Maincode imc) : base(imc)
        {

        }
    }
}
