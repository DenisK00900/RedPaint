using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class DialogCreateNewImage : DialogBox
    {
        public override DialogCreateNewImage Clone()
        {
            DialogCreateNewImage clone = new DialogCreateNewImage(mc, parent);

            SendCloneTo(clone);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(600f, 400f);
        }

        public DialogCreateNewImage(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
