using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public abstract class Hitbox
    {
        public AbstrEntity parent = null;

        public Vector2 pos {  get; set; }

        public abstract bool CheckLine(Vector2 pos1, Vector2 pos2);

        public abstract Vector2 GetCenter();

        public abstract float DistanceToCircleBoundary(Vector2 point);

        public Vector2 GetGlobalPos()
        {
            return parent != null ? parent.GetPos() + pos : pos;
        }

        public bool Check(Hitbox hb)
        {
            return CollisionSolver.Check(this, hb);
        }

        public abstract bool Check(Vector2 GlobalPoint);
    }
}
