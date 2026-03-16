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
    public class AlphaSet : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public CheckerTex checker;

        public FadeTex fade;

        public Vector2 size;

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

        public override void Update(float deltaTime)
        {
            Generate();

            (visual[0] as Sprite).texture = checker.Tex;
            (visual[1] as Sprite).texture = fade.Tex;

            base.Update(deltaTime);
        }

        public AlphaSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            checker = new CheckerTex(mc);
            fade = new FadeTex(mc);

            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            visual[0].origin = new Vector2(0f, 0f);

            visual[1] = new Sprite(this);
            visual[1].origin = new Vector2(0f, 0f);
        }
    }
}
