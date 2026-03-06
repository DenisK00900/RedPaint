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

            Panel[] panels = new Panel[5];
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i] = new Panel(ph.mc);
                panels[i].ChangeLocker();
            }

            ImageView iv = new ImageView(ph.mc);

            iv.SetPanel(panels[0]);

            ph.AddPanel(panels[0], ph.GetRect().GetSubrect(5, 4, 1, 0) + ph.GetRect().GetSubrect(5, 4, 3, 2));

            ph.AddPanel(panels[1], ph.GetRect().GetSubrect(5, 6, 0, 0) + ph.GetRect().GetSubrect(5, 6, 0, 3));

            ph.AddPanel(panels[2], ph.GetRect().GetSubrect(5, 6, 0, 4) + ph.GetRect().GetSubrect(5, 6, 0, 5));

            ph.AddPanel(panels[3], ph.GetRect().GetSubrect(5, 1, 4, 0));

            ph.AddPanel(panels[4], ph.GetRect().GetSubrect(5, 4, 1, 3) + ph.GetRect().GetSubrect(5, 4, 3, 3));
        }
    }
}
