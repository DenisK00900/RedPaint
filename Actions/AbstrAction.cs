using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public abstract class AbstrAction
    {
        public Maincode mc;
        public abstract void Act();

        public AbstrAction(Maincode imc)
        {
            mc = imc;
        }
    }
}
