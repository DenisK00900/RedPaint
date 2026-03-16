using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ToolSettings : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public ToolSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Настройка";
            depth = pl.baseRect.depth + 2;
        }
    }
}
