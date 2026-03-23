using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public interface IUseCloneFollows
    {
        public List<AbstrEntity> follows { get; set; }

        public void NewClone(AbstrEntity clone);
    }
}
