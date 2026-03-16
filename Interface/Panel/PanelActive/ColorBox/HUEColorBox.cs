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
    public class HUEColorBox : AbstrEntity
    {
        public HUEbox box;
        public AlphaSet alpha;
        public ColorSet color;

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(box);

            mc._entityManager.AddEntity(alpha);

            mc._entityManager.AddEntity(color);
        }

        public override void SetDepth(int depth)
        {
            base.SetDepth(depth);

            box.SetDepth(depth + 1);

            alpha.SetDepth(depth + 2);

            color.SetDepth(depth + 2);
        }

        public void SetSize(Vector2 size)
        {
            if (size.X < 1 || size.Y < 1)
            {
                box.hueColorTex.sizeX = 1;
                box.hueColorTex.sizeY = 1;

                alpha.size = new Vector2(0,0);

                return;
            }

            box.hueColorTex.sizeX = (int)size.X;
            box.hueColorTex.sizeY = (int)size.Y;
        }

        public void SetAlphaSize(Vector2 size)
        {
            alpha.SetSize(size);
        }

        public void SetColorSize(Vector2 size)
        {
            color.SetSize(size);
        }

        public HUEColorBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            box = new HUEbox(mc, this);

            alpha = new AlphaSet(mc, this);

            color = new ColorSet(mc, this);
        }
    }
}
