using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionPanelHolderDef : AbstrAction
    {
        public override void Act()
        {
            PanelHolderSettings.InitBasePanels(mc.mainHolder);
        }

        public ActionPanelHolderDef(Maincode imc) : base(imc)
        {
        }
    }
}
