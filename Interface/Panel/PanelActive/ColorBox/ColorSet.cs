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
    public class ColorSet : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public RainbowTex rainbow;

        public Vector2 size;

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

        public override void Update(float deltaTime)
        {
            Generate();

            (visual[0] as Sprite).texture = rainbow.Tex;

            base.Update(deltaTime);
        }

        public ColorSet(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            rainbow = new RainbowTex(mc);

            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
            visual[0].origin = new Vector2(0f, 0f);
        }
    }
}
