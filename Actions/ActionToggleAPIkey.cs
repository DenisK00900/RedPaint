using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionToggleAPIkey : AbstrAction
    {
        public bool useStandartKey;

        public override void Act()
        {
            mc._data.useStandratKey = useStandartKey;
        }

        public ActionToggleAPIkey(Maincode imc, bool useStrKey) : base(imc)
        {
            useStandartKey = useStrKey;
        }

    }
}
