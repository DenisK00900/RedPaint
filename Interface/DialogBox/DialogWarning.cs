using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RedPaint
{
    public class DialogWarning : DialogBox, IDrawable, IBlockInteraction
    {
        public Text message;
        public Text error;

        public bool isHardBlock { get; set; } = false;

        public bool isBlocking { get; set; } = false;
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public SimpleButton agree;
        public SimpleButton disagree;

        public override DialogError Clone()
        {
            DialogError clone = new DialogError(mc, message.text, error.text, parent);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            if (visual == null)
            {
                return new Vector2(500f, 300f);
            }
            else
            {
                return new Vector2(
                    (visual[1] as Text).GetRectSize().X + 32f,
                    180f + (visual[0] as Text).GetRectSize().Y + (visual[1] as Text).GetRectSize().Y
                    );
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public void SetAgreeText(string text)
        {
            (agree.visual[1] as Text).text = text;

            SetPosForButtons();
        }

        public void SetDisagreeText(string text)
        {
            (disagree.visual[1] as Text).text = text;

            SetPosForButtons();
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(agree);
            mc._entityManager.AddEntity(disagree);

            base.OnSpawn();
        }

        public override void SetDepth(int depth)
        {
            base.SetDepth(depth);

            baseRect.SetDepth(depth - 1);
            outline.SetDepth(depth - 2);
            setRect.SetDepth(depth);

            if (isCreated)
            {
                agree.SetDepth(depth + 1);
                disagree.SetDepth(depth + 1);
            }
        }

        private void SetPosForButtons()
        {
            agree.Resize();
            disagree.Resize();

            agree.SetPos(
                new Vector2(20f + agree.GetSize().X/2f, DetermentSize().Y - 40f - outlineSize.Y));
            disagree.SetPos(
                new Vector2(40f + agree.GetSize().X + disagree.GetSize().X / 2f, DetermentSize().Y - 40f - outlineSize.Y));
        }

        public override void OnDrop()
        {
            agree.UpdateHitbox();
            disagree.UpdateHitbox();

            base.OnDrop();
        }

        public DialogWarning(Maincode imc, string mess, string err, AbstrEntity pr = null) : base(imc, pr)
        {
            setRect.headText = "Предупреждение";

            agree = new SimpleButton(mc, baseRect);
            disagree = new SimpleButton(mc, baseRect);

            SetPosForButtons();

            message = new Text(baseRect);
            error = new Text(baseRect);

            message.text = mess;
            message.font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            message.origin = new Vector2(0.5f, 0f);

            error.text = err;
            error.font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            error.origin = new Vector2(0.5f, 0f);

            visual = new VisualElement[2];

            visual[0] = message;
            visual[1] = error;

            visual[0].pos = new Vector2(4f, 16f + DetermentSize().Y / 4f - 30f);
            visual[1].pos = new Vector2(4f, 16f + DetermentSize().Y / 2f - 30f);

            Vector2 size = DetermentSize();

            (baseRect.visual[0] as Sprite).scale = size - outlineSize;
            (outline.visual[0] as Sprite).scale = size;
            (setRect.visual[0] as Sprite).scale = new Vector2(size.X - outlineSize.X, 32);

            SetOnCenter();

            SetDepth(98);
        }
    }
}
