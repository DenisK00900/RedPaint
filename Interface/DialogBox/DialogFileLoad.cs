using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class DialogFileLoad : DialogBox
    {
        public Drawrect fileVievRect;
        public Drawrect fileVievRectOutLine;

        private Vector2 fileVievOutLineSize = new Vector2(4, 4);

        public override DialogFileLoad Clone()
        {
            DialogFileLoad clone = new DialogFileLoad(mc, parent);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(1000f, 750f);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(fileVievRect);
            mc._entityManager.AddEntity(fileVievRectOutLine);

            base.OnSpawn();
        }

        public DialogFileLoad(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            isSetPanel = false; 

            fileVievRect = new Drawrect(mc, baseRect);
            fileVievRectOutLine = new Drawrect(mc, fileVievRect);

            fileVievRect.visual[0].scale = DetermentSize() - new Vector2(40f, 120f);
            fileVievRect.visual[0].color = mc._settings.GetCurrPalletre().boxColor;

            fileVievRect.position = DetermentSize() / 2f - new Vector2(0f, 40f) - outlineSize/2f;

            fileVievRectOutLine.visual[0].scale = DetermentSize() - new Vector2(40f, 120f) + outlineSize;
            fileVievRectOutLine.visual[0].color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            fileVievRect.depth = baseRect.depth + 2;
            fileVievRectOutLine.depth = baseRect.depth + 1;
        }
    }
}
