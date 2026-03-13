using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionSelectTool : AbstrAction
    {
        AbstrTool tool;
        public override void Act()
        {
            mc._image.currTool = tool;
        }
        public ActionSelectTool(Maincode imc, AbstrTool t = null) : base(imc)
        {
            tool = t;
        }
    }
}
