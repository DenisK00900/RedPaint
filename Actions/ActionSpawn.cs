using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;


namespace RedPaint
{
    public class ActionSpawn : AbstrAction
    {
        public AbstrEntity follow;

        public Vector2 spawnOffset;

        public int spawnDepth;

        public override void Act()
        {
            AbstrEntity ae = follow.Clone();

            ae.SetPos(ae.position + spawnOffset);
            ae.SetDepth(ae is IDrawable d ? d.depth + spawnDepth : 0);

            mc._entityManager.AddEntity(ae);
        }

        public ActionSpawn(Maincode imc, AbstrEntity entity = null, int sd = 0) : base(imc)
        {
            follow = entity;

            spawnDepth = sd;
            spawnOffset = Vector2.Zero;
        }

        public ActionSpawn(Maincode imc, Vector2 offset, AbstrEntity entity = null, int sd = 0) : base(imc)
        {
            follow = entity;

            spawnDepth = sd;
            spawnOffset =  offset;
        }
    }
}
