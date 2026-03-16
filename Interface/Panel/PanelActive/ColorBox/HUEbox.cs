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
using System.Diagnostics;

namespace RedPaint
{
    public class HUEbox : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public HueColorTex hueColorTex;

        public bool isToss = false;

        public Vector2 lastPos = new Vector2(0,0);

        public void SetColor(Color newcolor)
        {
            hueColorTex.baseColor = newcolor;
        }

        public void UpdateHitbox()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(texSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos() - texSize / 2f;
        }

        public override void Update(float deltaTime)
        {
            hueColorTex.Generate();

            (visual[0] as Sprite).texture = hueColorTex.Tex;

            if (mouseOver)
            {
                if (mc._input.IsPressed(Button.LeftButton))
                {
                    isToss = true;
                }
            }
            if (mc._input.IsReleased(Button.LeftButton))
            {
                isToss = false;
            }

            if (isToss)
            {
                mc._image.SetColor(GetColor());
            }
        }

        public void ReselectColor()
        {
            mc._image.SetColor(TUH.GetPixelColor((visual[0] as Sprite).texture, lastPos).Value);
        }

        private float GetStateY()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            if (mc._input.GetMousePosition().Y <= rect.position.Y) return 0f;

            if (mc._input.GetMousePosition().Y >= rect.position.Y + rect.size.Y) return 1f;

            return (mc._input.GetMousePosition().Y - rect.position.Y) / rect.size.Y;
        }

        private float GetStateX()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            if (mc._input.GetMousePosition().X <= rect.position.X) return 0f;

            if (mc._input.GetMousePosition().X >= rect.position.X + rect.size.X) return 1f;

            return (mc._input.GetMousePosition().X - rect.position.X) / rect.size.X;
        }

        public Vector2 GetState()
        {
            return new Vector2(GetStateX(), GetStateY());
        }

        public Color GetColor()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            float posX = Math.Clamp(rect.size.X * GetStateX(), 0f, rect.size.X - 1f);
            float posY = Math.Clamp(rect.size.Y * GetStateY(), 0f, rect.size.Y - 1f);

            lastPos = new Vector2(posX, posY);

            return TUH.GetPixelColor((visual[0] as Sprite).texture, new Vector2(posX, posY)).Value;
        }

        public HUEbox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            hueColorTex = new HueColorTex(mc);

            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
        }
    }
}
