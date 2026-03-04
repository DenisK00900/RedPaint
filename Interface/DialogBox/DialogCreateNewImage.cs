using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class DialogCreateNewImage : DialogBox
    {
        public InputBox InputW;
        public InputBox InputH;
        public SpriteButton button;

        public override DialogCreateNewImage Clone()
        {
            DialogCreateNewImage clone = new DialogCreateNewImage(mc, parent);

            SendCloneTo(clone);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(140f, 200f);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(InputW);
            mc._entityManager.AddEntity(InputH);

            mc._entityManager.AddEntity(button);

            base.OnSpawn();
        }

        public override void OnDrop()
        {
            InputW.UpdateHitbox();
            InputH.UpdateHitbox();

            button.UpdateHitbox();
        }

        public DialogCreateNewImage(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            InputW = new InputBox(mc, baseRect);
            InputH = new InputBox(mc, baseRect);

            button = new CreateImageButton(mc, baseRect);

            Vector2 size = DetermentSize();

            float pos1 = InputW.DetermentSize().Y / 2f + 32f + InputH.outlineSize.Y / 2f + 12f;
            float pos2 = pos1 + InputH.outlineSize.Y / 2f + 48f;

            InputW.position = new Vector2(
                size.X / 2f - InputW.outlineSize.X / 2f, pos1);
            InputH.position = new Vector2(
                size.X / 2f - InputH.outlineSize.X / 2f, pos2);

            button.position = new Vector2(
                size.X / 2f - 4f, pos2 + 48f);

            InputW.SetDepth(baseRect.depth + 1);
            InputH.SetDepth(baseRect.depth + 1);
            button.SetDepth(baseRect.depth + 1);
        }
    }
}
