using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class PaletteBox : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public PaletteBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Палитра";
            depth = pl.baseRect.depth + 2;
        }
    }
}
