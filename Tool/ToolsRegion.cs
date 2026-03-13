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
    public class ToolsRegion : AbstrEntity
    {
        Drawrect outline;
        Drawrect baseRect;

        ToolsRegionSetRect setRect;

        public Vector2 outlineSize = new Vector2(8f, 8f);

        public List<AbstrTool> toolList = new List<AbstrTool>();

        public new ToolsBox parent;

        private float iconSize = 64f;

        public override ToolsRegion Clone()
        {
            ToolsRegion clone = new ToolsRegion(mc, parent);

            clone.baseRect = baseRect.Clone();
            clone.outline = outline.Clone();
            clone.setRect = setRect.Clone();
            clone.outlineSize = outlineSize;

            return clone;
        }

        public void SetHeadText(string text)
        {
            setRect.headText = text;

            (setRect.visual[1] as Text).text = setRect.headText;
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);
            mc._entityManager.AddEntity(setRect);
        }

        public Vector2 DetermentSize()
        {
            float sizeY = 1f;

            if (toolList.Count > 0)
            {
                sizeY = (toolList.Count * iconSize) % parent.panel.size.X;
            }

            return new Vector2(
                parent.panel.size.X, 
                sizeY * iconSize + 32f)
                - outlineSize;
        }

        public override void Update(float deltaTime)
        {
            baseRect.visual[0].scale = DetermentSize() - outlineSize;
            outline.visual[0].scale = DetermentSize();

            setRect.visual[0].scale = new Vector2(DetermentSize().X - outlineSize.X, 32f);

            setRect.SetPos(new Vector2(0f, -DetermentSize().Y/2f + 16f + outlineSize.Y/2f));

            (setRect.visual[1] as Text).pos = 
                new Vector2(-setRect.visual[0].scale.X/2f + (setRect.visual[1] as Text).GetRectSize().X / 2f + outlineSize.X, 0f);

            base.Update(deltaTime);
        }

        public override void SetDepth(int depth)
        {
            baseRect.depth = depth + 1;
            outline.depth = depth;
            setRect.depth = depth + 2;
        }

        public ToolsRegion(Maincode imc, ToolsBox pr) : base(imc, pr)
        {
            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);
            setRect = new ToolsRegionSetRect(mc, baseRect);

            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f),
            mc._settings.GetCurrPalletre().boxColor, 0.10f);

            (outline.visual[0] as Sprite).color =
            Color.Lerp(
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f),
            mc._settings.GetCurrPalletre().boxColor, 0.10f);
        }
    }
}
