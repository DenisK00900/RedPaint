using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public interface IWritable
    {
        public bool isWriting { get; set; }

        public bool includeNum { get; set; }

        public bool includeAlp { get; set; }

        public string stringInput { get; set; }
    }
}
