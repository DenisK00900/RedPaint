using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RedPaint
{
    public class ActionSpawn : AbstrAction
    {
        public AbstrEntity follow;

        public int spawnDepth;

        public override void Act()
        {
            AbstrEntity ae = follow.Clone();

            ae.SetDepth(spawnDepth);

            mc._entityManager.AddEntity(ae);
        }

        public ActionSpawn(Maincode imc, AbstrEntity entity = null, int sd = 0) : base(imc)
        {
            follow = entity;
            spawnDepth = sd;
        }
    }
}
