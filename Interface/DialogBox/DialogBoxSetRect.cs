using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Diagnostics;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class DialogBoxSetRect : Drawrect, IReactToMouse
    {
        private float timeUnderCloser = 0f;
        public float needTime = 0.25f;

        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public void UpdateHitbox()
        {
            hb = new Hitbox[2];

            Rect rect;

            rect = new Rect(visual[0].scale);
            rect.size.X -= 32f;
            hb[0] = new PolygonHitbox(rect);

            rect = new Rect(visual[0].scale);

            rect.size.X = 32f;
            hb[1] = new PolygonHitbox(rect);
            hb[1].pos = new Vector2((new Rect(visual[0].scale)).size.X - 32f, 0);

            for (int i = 0; i < hb.Length; i++)
            {
                hb[i].parent = this;
            }
        }

        public override void Update(float deltaTime)
        {
            visual[1].pos = new Vector2(visual[0].scale.X - 32f, 0);

            if(TUH.GetHitboxCollideIndex(hb, mc._input.GetMousePosition()) == 1)
                timeUnderCloser = Math.Clamp(timeUnderCloser + deltaTime, 0f, needTime);
            else
                timeUnderCloser = Math.Clamp(timeUnderCloser - deltaTime, 0f, needTime);

            visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, timeUnderCloser / needTime);

            base.Update(deltaTime);
        }

        public override void OnSpawn()
        {
            UpdateHitbox();
        }


        public DialogBoxSetRect(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");

            visual[1] = new Sprite(this);

            (visual[1] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/cross");
            (visual[1] as Sprite).origin = new Vector2(0f, 0f);
            (visual[1] as Sprite).color = mc._settings.GetCurrPalletre().textColor1;
            (visual[1] as Sprite).scale = new Vector2(0.5f, 0.5f);
        }
    }
}
