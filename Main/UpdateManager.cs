using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

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

            foreach (AbstrEntity entity in mc.entities)
            {
                if (entity is IReactToMouse rtm)
                {
                    rtm.mouseOver = false;
                    foreach (Hitbox hitbox in rtm.hb)
                    {
                        if (hitbox.Check(mousePosition)) rtm.mouseOver = true;
                    }
                }

                entity.Update(deltaTime);
            }
        }
    }
}
