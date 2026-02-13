using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace RedPaint
{
    public class BoardPanel : AbstrEntity
    {
        public Drawrect baseRect;

        public BoardPanel(Maincode mc)
        {
            baseRect = new Drawrect(mc);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color = mc._settings.GetCurrPalletre().baseColor2;
            (baseRect.visual[0] as Sprite).scale = new Vector2(mc._data.res.X, 60);

            SetPos(Vector2.Zero);

            mc._settings.AddEntity(baseRect);

            FileExpMenu expmenu1 = new FileExpMenu(mc, baseRect);
            expmenu1.SetPos(new Vector2(50, 30));

            mc._settings.AddEntity(expmenu1);
        }
    }
}
