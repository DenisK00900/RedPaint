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

            ph.ClearPanels();

            Panel[] panels = new Panel[5];
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i] = new Panel(ph.mc);
                panels[i].ChangeLocker();
            }

            ImageView iv = new ImageView(ph.mc);
            ToolsBox tb = new ToolsBox(ph.mc);
            ColorBox pb = new ColorBox(ph.mc);
            ToolSettings ts = new ToolSettings(ph.mc);
            LayerSettings ls = new LayerSettings(ph.mc);

            iv.SetPanel(panels[0]);

            ts.SetPanel(panels[1]);

            pb.SetPanel(panels[2]);

            tb.SetPanel(panels[3]);

            ls.SetPanel(panels[4]);

            ph.AddPanel(panels[0], ph.GetRect().GetSubrect(7, 4, 2, 0) + ph.GetRect().GetSubrect(7, 4, 5, 2));

            ph.AddPanel(panels[1], ph.GetRect().GetSubrect(7, 6, 0, 0) + ph.GetRect().GetSubrect(7, 6, 1, 2));

            ph.AddPanel(panels[2], ph.GetRect().GetSubrect(7, 6, 0, 3) + ph.GetRect().GetSubrect(7, 6, 1, 5));

            ph.AddPanel(panels[3], ph.GetRect().GetSubrect(7, 1, 6, 0));

            ph.AddPanel(panels[4], ph.GetRect().GetSubrect(7, 4, 2, 3) + ph.GetRect().GetSubrect(7, 4, 5, 3));
        }
    }
}
