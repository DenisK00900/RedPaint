using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PanelHolderSettings
    {
        public static void InitBasePanels(PanelHolder ph)
        {
            if (!ph.isCreated) throw new InvalidOperationException("PanelHolder должен быть добавлен как объект перед настройкой");

            Panel vp = new Panel(ph.mc);
            vp.ChangeLocker();

            ImageViev iv = new ImageViev(ph.mc);

            iv.SetPanel(vp);

            ph.mc._entityManager.AddEntity(vp);

            ph.AddPanel(vp, ph.GetRect().GetSubrect(5, 4, 1, 0) + ph.GetRect().GetSubrect(5, 4, 3, 2));
        }
    }
}
