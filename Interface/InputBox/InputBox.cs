using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class InputBox : AbstrEntity, IDrawable, IReactToMouse, IBlockInteraction, IWritable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }
        public Hitbox[] hb { get; set; }
        public bool mouseOver { get; set; }
        public bool isHardBlock { get; set; } = false;
        public bool isBlocking { get; set; } = false;

        public Drawrect box;
        public Drawrect outline;

        public Vector2 outlineSize = new Vector2(8f, 8f);

        public string stringInput { get; set; } = "";

        public bool isWriting { get; set; } = false;

        public bool includeNum { get; set; } = true;

        public bool includeAlp { get; set; } = false;

        private string shownString = "";

        private float dashTimer = 0f;
        private float dashCycle = 1.00f;

        public float boxSize = 96f;

        public virtual Vector2 DetermentSize()
        {
            return new Vector2(boxSize, 32f);
        }

        public void SetSize(float newsize = 96f)
        {
            boxSize = newsize;

            box.visual[0].scale = DetermentSize();
            outline.visual[0].scale = DetermentSize() + outlineSize;
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(box);
            mc._entityManager.AddEntity(outline);

            UpdateHitbox();
        }

        public override void SetDepth(int depth)
        {
            box.depth = depth + 1;
            outline.depth = depth;

            base.SetDepth(depth + 2);
        }

        public void UpdateHitbox()
        {
            Vector2 texSize = TUH.GetTextureSize((box.visual[0] as Sprite));

            hb = new Hitbox[1];
            hb[0] = new PolygonHitbox(new Rect(texSize));

            hb[0].parent = this;
            hb[0].isAbsoluite = true;

            hb[0].pos = GetPos() - texSize / 2f;
        }

        public override void Update(float deltaTime)
        {
            if (mouseOver && mc._input.IsPressed(Button.LeftButton))
            {
                isWriting = true;
                mc._input.LockOnWrite(this);
            }
            else if (
                mc._input.IsPressed(Keys.Enter) ||
                (!mouseOver && (mc._input.IsPressed(Button.LeftButton) || mc._input.IsPressed(Button.RightButton)))
                )
            {
                isWriting = false;
                mc._input.UnlockWrite();
            }

            if (isWriting)
            {
                dashTimer = (dashTimer + deltaTime) % (dashCycle);
            }
            else
            {
                dashTimer = 0f;
            }

            isBlocking = isWriting;

            shownString = stringInput;

            if (dashTimer > dashCycle/2f) shownString += "|";

            (visual[0] as Text).text = shownString;

            (visual[0] as Text).pos = new Vector2(
                (float)Math.Round(4f -DetermentSize().X/2 +
                TUH.GetSizeFromText(shownString, (visual[0] as Text).font).X/2f),
                0f);
        }

        public InputBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).color = mc._settings.GetCurrPalletre().textColor2;

            box = new Drawrect(mc, this);
            outline = new Drawrect(mc, box);

            box.visual[0].scale = DetermentSize();
            box.visual[0].color = mc._settings.GetCurrPalletre().boxColor;

            outline.visual[0].scale = DetermentSize() + outlineSize;
            outline.visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().boxColor, 0.25f);
        }
    }
}
