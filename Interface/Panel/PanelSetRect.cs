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
    public class PanelSetRect : Drawrect, IReactToMouse
    {

        private Texture2D lockIcon;

        private Texture2D unlockIcon;

        public Rect targetRect;

        private float timeUnderLocker = 0f;
        private float timeUnderCloser = 0f;
        public float needTime = 0.25f;

        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; } = false;

        public void ChangeLockerIcon(bool locker)
        {
            (visual[2] as Sprite).texture = locker ? lockIcon : unlockIcon;
        }

        public void UpdateHitbox()
        {
            hb = new Hitbox[3];

            Rect rect;

            rect = targetRect.Clone();
            rect.size.X -= 64f;
            hb[0] = new PolygonHitbox(rect);

            rect = targetRect.Clone();

            rect.size.X = 32f;
            hb[1] = new PolygonHitbox(rect);
            hb[1].pos = new Vector2(targetRect.size.X - 64f, 0);

            rect = targetRect.Clone();

            rect.size.X = 32f;
            hb[2] = new PolygonHitbox(rect);
            hb[2].pos = new Vector2(targetRect.size.X - 32f, 0);

            for (int i = 0; i < hb.Length; i++)
            {
                hb[i].parent = this;
            }
        }

        public override void Update(float deltaTime)
        {
            visual[1].pos = new Vector2(visual[0].scale.X - 32f, 0);
            visual[2].pos = new Vector2(visual[0].scale.X - 64f, 0);

            if (TUH.GetHitboxCollideIndex(hb, mc._input.GetMousePosition()) == 1)
                timeUnderLocker = Math.Clamp(timeUnderLocker + deltaTime, 0f, needTime);
            else
                timeUnderLocker = Math.Clamp(timeUnderLocker - deltaTime, 0f, needTime);

            if (TUH.GetHitboxCollideIndex(hb, mc._input.GetMousePosition()) == 2)
                timeUnderCloser = Math.Clamp(timeUnderCloser + deltaTime, 0f, needTime);
            else
                timeUnderCloser = Math.Clamp(timeUnderCloser - deltaTime, 0f, needTime);

            visual[2].color = 
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, timeUnderLocker / needTime);

            visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().effectColor1, timeUnderCloser / needTime);

            base.Update(deltaTime);
        }

        public PanelSetRect(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[3];
            for (int i = 0; i < visual.Length; i++)
            {
                visual[i] = new Sprite(this);
            }

            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");
            (visual[0] as Sprite).origin = Vector2.Zero;
            (visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.50f);

            (visual[1] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/cross");
            (visual[1] as Sprite).origin = new Vector2(0f, 0f);
            (visual[1] as Sprite).color = mc._settings.GetCurrPalletre().textColor1;
            (visual[1] as Sprite).scale = new Vector2(0.5f, 0.5f);

            lockIcon = mc.Content.Load<Texture2D>("Texture/Icons/lock");
            unlockIcon = mc.Content.Load<Texture2D>("Texture/Icons/unlock");

            (visual[2] as Sprite).texture = unlockIcon;
            (visual[2] as Sprite).origin = new Vector2(0f, 0f);
            (visual[2] as Sprite).color = mc._settings.GetCurrPalletre().textColor1;
            (visual[2] as Sprite).scale = new Vector2(0.5f, 0.5f);
        }
    }
}
