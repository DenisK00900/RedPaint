using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class InputBox : AbstrEntity, IDrawable, IReactToMouse
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }

        public Drawrect box;
        public Drawrect outline;

        public Vector2 outlineSize = new Vector2(8f, 8f);

        public string stringInput = "";

        public bool IsWriting = false;

        public virtual Vector2 DetermentSize()
        {
            return new Vector2(96f, 32f);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(box);
            mc._entityManager.AddEntity(outline);
        }

        public override void SetDepth(int depth)
        {
            box.depth = depth;
            outline.depth = depth - 1;
        }

        public void UpdateHitbox()
        {
            
        }

        public InputBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            box = new Drawrect(mc, this);
            outline = new Drawrect(mc, box);

            Vector2 size = DetermentSize();

            box.visual[0].scale = size;
            box.visual[0].color = mc._settings.GetCurrPalletre().boxColor;

            outline.visual[0].scale = size + outlineSize;
            outline.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().boxColor, 0.25f);
        }
    }
}
