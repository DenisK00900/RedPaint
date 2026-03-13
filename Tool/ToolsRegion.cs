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
    public class ToolsRegion : AbstrEntity
    {
        Drawrect outline;
        Drawrect baseRect;

        ToolsRegionSetRect setRect;

        public Vector2 outlineSize = new Vector2(8f, 8f);

        public List<ToolViewBox> toolList = new List<ToolViewBox>();

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

        public void AddTool(AbstrTool tool)
        {
            ToolViewBox box = new ToolViewBox(mc);
            box.SetTool(tool, iconSize);
            box.parent = baseRect;
            box.SetDepth(baseRect.depth + 6);
            box.isAbsolute = true;

            mc._entityManager.AddEntity(box);

            toolList.Add(box);
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
            float contentWidth = parent.panel.size.X - outlineSize.X - parent.panel.outlineSize.X;

            int itemsPerRow = Math.Max(1, (int)(contentWidth / iconSize));

            int rows = 0;
            if (toolList.Count > 0)
            {
                rows = (int)Math.Ceiling((float)toolList.Count / itemsPerRow);
            }

            float contentHeight = rows * iconSize + 32f;

            return new Vector2(
                parent.panel.size.X - parent.panel.outlineSize.X,
                contentHeight + outlineSize.Y
            );
        }

        public void UpdateBoxPos(int index)
        {
            ToolViewBox box = toolList[index];


            float contentWidth = baseRect.visual[0].scale.X;

            int itemsPerRow = Math.Max(1, (int)(contentWidth / iconSize));

            int row = index / itemsPerRow;
            int col = index % itemsPerRow;

            Vector2 topLeft = baseRect.GetPos() - baseRect.visual[0].scale / 2f;

            float offsetX = iconSize / 2f;
            float offsetY = 32f + iconSize / 2f;

            Vector2 newPos = topLeft + new Vector2(offsetX, offsetY) + new Vector2(col * iconSize, row * iconSize);

            box.targetPos = newPos;
        }

        public override void Update(float deltaTime)
        {
            baseRect.visual[0].scale = DetermentSize() - outlineSize;
            outline.visual[0].scale = DetermentSize();

            setRect.visual[0].scale = new Vector2(DetermentSize().X - outlineSize.X, 32f);

            setRect.SetPos(new Vector2(0f, -DetermentSize().Y/2f + 16f + outlineSize.Y/2f));

            (setRect.visual[1] as Text).pos = 
                new Vector2(-setRect.visual[0].scale.X/2f + (setRect.visual[1] as Text).GetRectSize().X / 2f + outlineSize.X, 0f);

            for(int i = 0; i < toolList.Count; i++)
            {
                UpdateBoxPos(i);
            }

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
