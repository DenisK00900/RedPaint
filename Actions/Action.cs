using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public abstract class Action
    {
        public Maincode mc;
        public abstract void Act();

        public Action(Maincode imc)
        {
            mc = imc;
        }
    }
}
