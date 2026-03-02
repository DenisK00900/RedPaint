using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class DialogFileLoad : DialogBox
    {
        public Drawrect fileVievRect;
        public Drawrect fileVievRectOutLine;

        public Drawrect fileVievTop;

        private Vector2 fileVievOutLineSize = new Vector2(4, 4);

        public string currDir = "";

        public List<TextButton> dir = new List<TextButton>();
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
            mc._entityManager.AddEntity(fileVievTop);

            UpdateListInfo();

            base.OnSpawn();
        }

        public void UpdateListInfo(string ch = "")
        {
            foreach (TextButton button in dir)
            {
                button.Destroy();
            }

            dir = new List<TextButton>();

            currDir = currDir + ch;

            float currY = 16f;

            List<String> contDir = FileBrowserSolver.GetDirectoryContents(currDir);

            foreach (String cont in contDir)
            {
                TextButton opts = new TextButton(mc, fileVievRect);

                Text tx = new Text(null);

                tx.font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
                tx.text = cont;
                tx.origin = new Vector2(0f, 0f);

                currY += tx.GetRectSize().Y + 4f;

                opts.action = new ActionFileBrowser(mc, this, cont);
                opts.SetPos(
                    new Vector2(
                        -fileVievRect.visual[0].scale.X / 2f + 8f, 
                        -fileVievRect.visual[0].scale.Y / 2f + currY)
                    );
                opts.origColor = mc._settings.GetCurrPalletre().textColor2;
                opts.effColor = mc._settings.GetCurrPalletre().effectColor2;
                opts.SetText(tx);

                dir.Add(opts);
            }

            foreach (TextButton button in dir)
            {
                mc._entityManager.AddEntity(button);
            }
        }

        public DialogFileLoad(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            isSetPanel = false; 

            fileVievRect = new Drawrect(mc, baseRect);
            fileVievRectOutLine = new Drawrect(mc, fileVievRect);
            fileVievTop = new Drawrect(mc, fileVievRect);

            fileVievRect.visual[0].scale = DetermentSize() - new Vector2(40f, 120f);
            fileVievRect.visual[0].color = mc._settings.GetCurrPalletre().boxColor;

            fileVievRect.position = DetermentSize() / 2f - new Vector2(0f, 40f) - outlineSize/2f;

            fileVievRectOutLine.visual[0].scale = DetermentSize() - new Vector2(40f, 120f) + outlineSize;
            fileVievRectOutLine.visual[0].color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            fileVievTop.visual[0].scale = new Vector2(DetermentSize().X- 40f, 32f);
            fileVievTop.visual[0].color =
            Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.15f);
            fileVievTop.position = new Vector2(0f, (-fileVievRectOutLine.visual[0].scale.Y + fileVievTop.visual[0].scale.Y) / 2 + 4);

            fileVievRect.depth = baseRect.depth + 2;
            fileVievRectOutLine.depth = baseRect.depth + 1;
            fileVievTop.depth = fileVievRect.depth + 1;
        }
    }
}
