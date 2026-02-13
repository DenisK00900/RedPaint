using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class EntityManager
    {
        Maincode mc;
        public EntityManager(Maincode parent)
        {
            mc = parent;
        }

        public List<AbstrEntity> addBuffer;

        public void Apply()
        {

        }
    }
}
