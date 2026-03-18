using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ToolSetBool : ToolSet
    {
        public bool value;

        public override T GetValue<T>()
        {
            return (T)(object)value;
        }

        public ToolSetBool(Maincode mc, AbstrEntity pr) : base(mc, pr)
        {

        }
    }
}
