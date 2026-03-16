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
    public class AlphaSet : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public CheckerTex checker;

        public FadeTex fade;

        public Vector2 size;

        public bool isToss = false;

        public void Generate()
        {
            checker.sizeX = size.X < 1 ? 1 : (int)Math.Round(size.X);
            checker.sizeY = size.Y < 1 ? 1 : (int)Math.Round(size.Y);

            checker.sizeChecker = 10;

            checker.color1 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor1, 0.9f);
            checker.color2 = Color.Lerp(Color.Gray, mc._settings.GetCurrPalletre().baseColor2, 0.9f);

            checker.Generate();

            fade.sizeX = size.X < 1 ? 1 : (int)Math.Round(size.X);
            fade.sizeY = size.Y < 1 ? 1 : (int)Math.Round(size.Y);
            fade.Generate();
        }

        public void SetSize(Vector2 size)
        {
            this.size = size;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (visual == null) return;

            var device = mc.GraphicsDevice;

            var oldRenderTarget = device.GetRenderTargets();
            var oldRasterizerState = device.RasterizerState;
            var oldBlendState = device.BlendState;
            var oldSamplerState = device.SamplerStates[0];

            sb.End();

            sb.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.NonPremultiplied,
                    SamplerState.PointClamp,
                    null,
                    null
                    );

            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }

            sb.End();

            device.SetRenderTargets(oldRenderTarget);
            device.RasterizerState = oldRasterizerState;
            device.BlendState = oldBlendState;
            device.SamplerStates[0] = oldSamplerState;

            sb.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null
            );
        }

        public override void Update(float deltaTime)
        {
            Generate();

            (visual[0] as Sprite).texture = checker.Tex;
            (visual[1] as Sprite).texture = fade.Tex;
            visual[1].color = new Color(
                        mc._image.GetColor().R,
                        mc._image.GetColor().G,
                        mc._image.GetColor().B,
                        1f);

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
                mc._image.SetAlpha(GetState());
            }

            base.Update(deltaTime);
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

        public float GetState()
        {
            Vector2 texSize = TUH.GetTextureSize((visual[0] as Sprite));

            Rect rect = new Rect(GetPos() - texSize / 2f, texSize);

            if (mc._input.GetMousePosition().Y <= rect.position.Y) return 0f;

            if (mc._input.GetMousePosition().Y >= rect.position.Y + rect.size.Y) return 1f;

            return (mc._input.GetMousePosition().Y - rect.position.Y) / rect.size.Y;
        }

        public AlphaSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            checker = new CheckerTex(mc);
            fade = new FadeTex(mc);

            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            visual[1] = new Sprite(this);

            UpdateHitbox();
        }
    }
}
