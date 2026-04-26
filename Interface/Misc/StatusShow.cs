using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class StatusShow : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        private string text = "Нет логов";

        private float fadeTimer = 0f;

        private float fadeTimeNeed = 10f;

        public bool isNoFade = false;

        public void SetText(string newtext)
        {
            text = newtext;
            fadeTimer = 0f;
        }

        public override StatusShow Clone()
        {
            StatusShow clone = new StatusShow(mc, parent);

            return clone;
        }

        public override void Update(float deltaTime)
        {
            fadeTimer = Math.Clamp(fadeTimer + deltaTime, 0f, fadeTimeNeed);

            (visual[0] as Text).alpha = isNoFade ? 1f : 1f - (fadeTimer/fadeTimeNeed);
            (visual[0] as Text).text = text;
            (visual[0] as Text).pos = new Vector2(mc._data.res.X - (visual[0] as Text).GetRectSize().X - 30f, 30f -
                (visual[0] as Text).GetRectSize().Y/2f);
        }

        public StatusShow(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            if (mc._status.show != null)
            {
                throw new Exception("StatusShow уже существует. Создание нескольких не допускается.");
            }

            depth = int.MaxValue;

            visual = new VisualElement[1];

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).origin = new Vector2(0f, 0f);
            (visual[0] as Text).isAbsolute = true;
            (visual[0] as Text).color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.5f);

            mc._status.show = this;

            fadeTimer = fadeTimeNeed;
        }
    }
}
