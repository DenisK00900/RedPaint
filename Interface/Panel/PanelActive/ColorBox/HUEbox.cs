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

namespace RedPaint
{
    public class HUEbox : AbstrEntity, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public HueColorTex hueColorTex;



        public override void Update(float deltaTime)
        {
            hueColorTex.Generate();

            (visual[0] as Sprite).texture = hueColorTex.Tex;
        }

        public HUEbox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            hueColorTex = new HueColorTex(mc);

            visual = new VisualElement[1];

            visual[0] = new Sprite(this);
        }
    }
}
