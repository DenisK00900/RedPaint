using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionNewLayer : AbstrAction
    {
        public override void Act()
        {
            mc._image.AddLayer();
        }

        public ActionNewLayer(Maincode imc) : base(imc)
        {
        }
    }
}
