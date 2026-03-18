using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public abstract class ToolSet : AbstrEntity
    {
        public string name = "Настройка";

        public abstract T GetValue<T>();

        public ToolSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
