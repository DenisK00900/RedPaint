using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint.Base
{
    public interface IHint
    {
        public Hitbox[] hb { get; set; }

        public bool mouseOver { get; set; }

        public float mouseOverStopTime { get; set; }

        public float mouseOverStopTimeNeed {  get; set; }

        public bool hintShow { get; set; }

        public Hint hint { get; set; }
    }
}
