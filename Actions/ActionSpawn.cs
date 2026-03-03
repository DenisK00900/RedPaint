using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RedPaint
{
    public class ActionSpawn : AbstrAction
    {
        public AbstrEntity follow;

        public override void Act()
        {
            mc._entityManager.AddEntity(follow.Clone());
        }

        public ActionSpawn(Maincode imc, AbstrEntity entity = null) : base(imc)
        {
            follow = entity;
        }
    }
}
