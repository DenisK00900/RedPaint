using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PopMenu : AbstrEntity
    {
        public Drawrect baseRect;

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(baseRect);
        }

        public override PopMenu Clone()
        {
            PopMenu clone = new PopMenu(mc, GetPos(), parent);

            clone.SetPos(GetPos());
            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            //clone.baseRect = baseRect.Clone(); Избегать двойного клонирования!!!

            return clone;
        }

        public PopMenu(Maincode mc, Vector2 pos, AbstrEntity pr = null) : base(mc, pos, pr)
        {
            baseRect = new Drawrect(mc, this);

            SetPos(pos);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;

            (baseRect.visual[0] as Sprite).color = 
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.3f);

            (baseRect.visual[0] as Sprite).scale = new Vector2(200, 400);
        }
    }
}
