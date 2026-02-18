using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Diagnostics;

namespace RedPaint
{
    public class UpdateManager
    {
        public Maincode mc;

        public UpdateManager(Maincode parent)
        {
            mc = parent;
        }

        public void Update(float deltaTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                TUH.PrintEntityHierarchy(mc);
            }

            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            List<Hitbox> overlap = new List<Hitbox>();

            foreach (AbstrEntity entity in mc.entities)
            {
                if (entity is IReactToMouse rtm)
                {
                    rtm.mouseOver = false;

                    foreach (Hitbox hitbox in rtm.hb)
                    {
                        if (hitbox.Check(mousePosition)) overlap.Add(hitbox);
                    }
                }
            }

            if (overlap.Count > 0) (overlap.OrderByDescending(h => h.depth).First().parent as IReactToMouse).mouseOver = true;

            foreach (AbstrEntity entity in mc.entities)
            {
                    entity.Update(deltaTime);
            }
        }
    }
}
