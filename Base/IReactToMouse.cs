using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public interface IReactToMouse
    {
        public Hitbox[] hb { get; set; }
        
        public bool mouseOver { get; set; }
    }
}
