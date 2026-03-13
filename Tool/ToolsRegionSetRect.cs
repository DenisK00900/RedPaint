using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ToolsRegionSetRect : Drawrect
    {
        public string headText = "Без названия";

        public override ToolsRegionSetRect Clone()
        {
            ToolsRegionSetRect clone = new ToolsRegionSetRect(mc, parent);

            clone.headText = headText;

            return clone;
        }

        public ToolsRegionSetRect(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            visual[0].color =
            Color.Lerp(
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.50f),
            mc._settings.GetCurrPalletre().boxColor, 0.10f);

            visual[1] = new Text(this);
            (visual[1] as Text).text = headText;
            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            visual[1].color = Color.Lerp
                (mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.5f);
        }
    }
}
