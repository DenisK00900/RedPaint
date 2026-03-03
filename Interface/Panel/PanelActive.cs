using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public abstract class PanelActive : AbstrEntity
    {
        public Rect activeRect;

        public Panel panel;

        public virtual void SetPanel(Panel pl)
        {
            panel = pl;

            panel.panelActive = this;

            parent = panel;
        }

        public override void Update(float deltaTime)
        {
            if (panel != null) activeRect = panel.GetActiveRect();

            base.Update(deltaTime);
        }

        public PanelActive(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
