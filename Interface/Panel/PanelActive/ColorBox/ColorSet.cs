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
    public class ColorSet : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public RainbowTex rainbow;

        public Vector2 size;

        public bool isToss = false;

        public Color selectColor = Color.Red;

        public void Generate()
        {
            rainbow.sizeX = size.X < 1 ? 1 : (int)Math.Round(size.X);
            rainbow.sizeY = size.Y < 1 ? 1 : (int)Math.Round(size.Y);

            rainbow.Generate();
        }

        public void SetSize(Vector2 size)
        {
            this.size = size;
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
            Generate();

            (visual[0] as Sprite).texture = rainbow.Tex;

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
                selectColor = GetColor();

                if (parent is HUEColorBox pr)
                {
                    pr.box.SetColor(selectColor);

                    pr.box.ReselectColor();
                }
            }

            base.Update(deltaTime);
        }

        public float GetState()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            if (mc._input.GetMousePosition().Y <= rect.position.Y) return 0f;

            if (mc._input.GetMousePosition().Y >= rect.position.Y + rect.size.Y) return 1f;

            return (mc._input.GetMousePosition().Y - rect.position.Y) / rect.size.Y;
        }

        public Color GetColor()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            float pos = Math.Clamp(rect.size.Y * GetState(), 0f, rect.size.Y-1f);

            return TUH.GetPixelColor((visual[0] as Sprite).texture, new Vector2(1f, pos)).Value;
        }

        public ColorSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            rainbow = new RainbowTex(mc);

            visual = new VisualElement[1];

            visual[0] = new Sprite(this);

            rainbow.Generate();

            (visual[0] as Sprite).texture = rainbow.Tex;
        }
    }
}
