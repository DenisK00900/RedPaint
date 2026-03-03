using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RedPaint
{
    public class DialogMessage : DialogBox, IDrawable, IBlockInteraction
    {
        public Text message;
        public Text error;

        public bool isHardBlock { get; set; } = false;
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override DialogMessage Clone()
        {
            DialogMessage clone = new DialogMessage(mc, message.text, error.text, parent);

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
                    60f + (visual[0] as Text).GetRectSize().Y + (visual[1] as Text).GetRectSize().Y
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

        public DialogMessage(Maincode imc, string mess, string err, AbstrEntity pr = null) : base(imc, pr)
        {
            isSetPanel = true;

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

            visual[0].pos = new Vector2(4f, 16f + DetermentSize().Y / 4f);
            visual[1].pos = new Vector2(4f, 16f + DetermentSize().Y / 2f);

            depth = 98;
            baseRect.depth = depth - 1;
            outline.depth = depth - 2;
            setRect.depth = depth;

            Vector2 size = DetermentSize();

            (baseRect.visual[0] as Sprite).scale = size - outlineSize;
            (outline.visual[0] as Sprite).scale = size;
            (setRect.visual[0] as Sprite).scale = new Vector2(size.X - outlineSize.X, 32);

            SetOnCenter();
        }
    }
}
