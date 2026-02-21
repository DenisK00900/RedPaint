using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class VersionShow : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public VersionShow(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            depth = 99999;

            visual = new VisualElement[1];

            visual[0] = new Text(this);

            (visual[0] as Text).text = mc._data.version;
            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).origin = new Vector2(0f,0f);
            (visual[0] as Text).pos = -(visual[0] as Text).GetRectSize() + mc._data.res;
            (visual[0] as Text).isAbsolute = true;
            (visual[0] as Text).color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.5f);
        }
    }
}
