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
        private KeyboardState _prevKeyboardState;

        public UpdateManager(Maincode parent)
        {
            mc = parent;
            _prevKeyboardState = Keyboard.GetState();
        }

        public void Update(float deltaTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mc._data.isDevToolsOn)
            {
                if (currKeyboardState.IsKeyDown(Keys.F3) && _prevKeyboardState.IsKeyUp(Keys.F3))
                {
                    TUH.PrintEntityHierarchy(mc);
                }

                if (currKeyboardState.IsKeyDown(Keys.F4) && _prevKeyboardState.IsKeyUp(Keys.F4))
                {
                    Panel newpanel = new Panel(mc);
                    mc._entityManager.AddEntity(newpanel);
                    mc.mainHolder.AddPanel(newpanel, mousePosition);
                }
            }

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

            _prevKeyboardState = currKeyboardState;
        }
    }
}