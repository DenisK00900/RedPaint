using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RedPaint.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class Slider : AbstrEntity, IReactToMouse
    {
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        Drawrect line;
        Drawrect border1;
        Drawrect border2;
        Drawrect push;

        bool isTaken = false;

        public float lean;

        public float leight = 200f;

        private void UpdatePushPos()
        {
            push.SetPos(new Vector2(Math.Clamp(mc._input.GetMousePosition().X - GetPos().X, -leight/2f, leight / 2f),0f));
        }

        public override void Update(float deltaTime)
        {
            UpdateHitbox();

            if (mouseOver && mc._input.IsPressed(Button.LeftButton))
            {
                isTaken = true;
            }
            
            if (mc._input.IsReleased(Button.LeftButton))
            {
                isTaken = false;
            }

            if (isTaken)
            {
                UpdatePushPos();

                lean = TUH.InverseLerp(-leight / 2f, leight / 2f, push.position.X);
            }
        }

        public override void SetDepth(int depth)
        {
            line.SetDepth(depth);
            border1.SetDepth(depth);
            border2.SetDepth(depth);

            push.SetDepth(depth + 1);
        }

        public void SetDef(float def)
        {
            lean = def;

            push.SetPos(new Vector2(MathHelper.Lerp(-leight / 2f, leight / 2f, def),0f));
        }

        public void UpdateHitbox()
        {
            Vector2 texSize = TUH.GetTextureSize((push.visual[0] as Sprite));

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(texSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = push.GetPos() - texSize / 2f;
        }

        public void SetLeight(float newl)
        {
            leight = newl;

            line.visual[0].scale = new Vector2(leight, 8f);

            border1.SetPos(new Vector2(-(leight / 2f + 8f), 0f));
            border2.SetPos(new Vector2((leight / 2f + 8f), 0f));
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(line);
            mc._entityManager.AddEntity(border1);
            mc._entityManager.AddEntity(border2);

            mc._entityManager.AddEntity(push);
        }

        public Slider(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            line = new Drawrect(mc, this);
            border1 = new Drawrect(mc, this);
            border2 = new Drawrect(mc, this);

            push = new Drawrect(mc, this);

            line.visual[0].scale = new Vector2(leight, 8f);
            border1.visual[0].scale = new Vector2(8f, 32f);
            border2.visual[0].scale = new Vector2(8f, 32f);

            border1.SetPos(new Vector2(-(leight/2f + 8f),0f));
            border2.SetPos(new Vector2((leight / 2f + 8f), 0f));

            push.visual[0].scale = new Vector2(8f, 24f);

            line.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.8f);

            border1.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.8f);

            border2.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.8f);

            push.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.2f);
        }
    }
}
