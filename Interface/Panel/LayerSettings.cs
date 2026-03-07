using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class LayerSettings : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public LayerSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Слой";
            depth = pl.baseRect.depth + 2;
        }
    }
}
