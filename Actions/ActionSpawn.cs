using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionSpawn : Action
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
