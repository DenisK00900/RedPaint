using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace RedPaint
{
    public class DialogFileLoad : DialogBox
    {
        public Drawrect fileViewRect;
        public Drawrect fileViewRectOutLine;
        public Drawrect fileViewTop;
        public Drawrect fileViewDecor1;
        public Drawrect fileViewDecor2;
        public Drawrect fileViewDecor3;
        public Drawrect fileViewSide;

        public FileLoadUpFolderButton upFolderButton;

        private readonly Vector2 fileViewOutLineSize = new Vector2(4, 4);
        private readonly float columnOffsetName = 0f;
        private readonly float columnOffsetType = 330f;
        private readonly float columnOffsetStatus = 440f;
        private readonly float columnOffsetDecor3 = 600f;
        private readonly float listPadding = 40f;
        private readonly float listSpacing = 4f;
        private readonly float listStartXOffset = 8f;

        public string currDir = "";
        public List<TextButton> dir = new List<TextButton>();
        public List<TextButton> dirType = new List<TextButton>();
        public List<TextButton> dirError = new List<TextButton>();

        private SpriteFont _font;

        public override DialogFileLoad Clone()
        {
            DialogFileLoad clone = new DialogFileLoad(mc, parent);
            clone.currDir = this.currDir;
            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(1000f, 750f);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(fileViewRect);
            mc._entityManager.AddEntity(fileViewRectOutLine);
            mc._entityManager.AddEntity(fileViewTop);
            mc._entityManager.AddEntity(fileViewDecor1);
            mc._entityManager.AddEntity(fileViewDecor2);
            mc._entityManager.AddEntity(fileViewDecor3);
            mc._entityManager.AddEntity(fileViewSide);

            mc._entityManager.AddEntity(upFolderButton);

            UpdateListInfo();
            base.OnSpawn();
        }

        public void FolderUp()
        {
            Debug.WriteLine("flag1 " + currDir);

            currDir = FileBrowserSolver.GetParentDirectory(currDir);

            Debug.WriteLine("flag2 " + currDir);

            UpdateListInfo(currDir, true);
        }

        public void UpdateListInfo(string ch = "", bool setMode = false)
        {
            ClearLists();

            currDir = setMode ? ch : currDir + ch;

            Vector2 rectScale = fileViewRect.visual[0].scale;
            float currentY = listPadding;
            float baseX = -rectScale.X / 2f + listStartXOffset;
            float baseY = -rectScale.Y / 2f;

            List<string> contDir = FileBrowserSolver.GetDirectoryContents(currDir);

            foreach (string cont in contDir)
            {
                string fileName = FileBrowserSolver.ShortenString(cont, 24);
                string fileType = FileBrowserSolver.ShortenString(FileBrowserSolver.GetTypeOfPath(cont), 16);
                string fileStatus = FileBrowserSolver.ShortenString(FileBrowserSolver.CanOpenPath(currDir + cont), 16);

                Text textName = CreateText(fileName);
                Text textType = CreateText(fileType);
                Text textStatus = CreateText(fileStatus);

                float itemHeight = textName.GetRectSize().Y + listSpacing;

                Vector2 posName = new Vector2(baseX + columnOffsetName, baseY + currentY);
                Vector2 posType = new Vector2(baseX + columnOffsetType, baseY + currentY);
                Vector2 posStatus = new Vector2(baseX + columnOffsetStatus, baseY + currentY);

                TextButton btnName = CreateButton(posName, textName, cont, new ActionFileBrowser(mc, this, cont));
                TextButton btnType = CreateButton(posType, textType, cont);
                TextButton btnStatus = CreateButton(posStatus, textStatus, cont);

                dir.Add(btnName);
                dirType.Add(btnType);
                dirError.Add(btnStatus);

                currentY += itemHeight;
            }

            AddListsToEntityManager();
        }

        public DialogFileLoad(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            isSetPanel = false;
            _font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");

            fileViewRect = new Drawrect(mc, baseRect);
            fileViewRectOutLine = new Drawrect(mc, fileViewRect);
            fileViewTop = new Drawrect(mc, fileViewRect);
            fileViewDecor1 = new Drawrect(mc, fileViewRect);
            fileViewDecor2 = new Drawrect(mc, fileViewRect);
            fileViewDecor3 = new Drawrect(mc, fileViewRect);
            fileViewSide = new Drawrect(mc, fileViewRect);

            upFolderButton = new FileLoadUpFolderButton(mc, fileViewTop);

            Vector2 size = DetermentSize();
            Vector2 innerSize = size - new Vector2(40f, 120f);

            fileViewRect.visual[0].scale = innerSize;
            fileViewRect.visual[0].color = mc._settings.GetCurrPalletre().boxColor;
            fileViewRect.position = size / 2f - new Vector2(0f, 40f) - outlineSize / 2f;

            fileViewRectOutLine.visual[0].scale = innerSize + outlineSize;
            fileViewRectOutLine.visual[0].color = Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            fileViewTop.visual[0].scale = new Vector2(size.X - 40f, 32f);
            fileViewTop.visual[0].color = Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.25f);
            fileViewTop.position = new Vector2(0f, (-fileViewRect.visual[0].scale.Y + fileViewTop.visual[0].scale.Y) / 2);

            float decorHeight = fileViewRect.visual[0].scale.Y - 32f;

            SetupDecor(fileViewDecor1, 4f, decorHeight, columnOffsetType, 0.05f);
            SetupDecor(fileViewDecor2, 4f, decorHeight, columnOffsetStatus, 0.05f);
            SetupDecor(fileViewDecor3, 4f, decorHeight, columnOffsetDecor3, 0.05f);

            fileViewSide.visual[0].scale = new Vector2(32f, decorHeight);
            fileViewSide.visual[0].color = Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, 0.15f);
            fileViewSide.position = new Vector2(fileViewRect.visual[0].scale.X / 2 - 16f, 16f);

            upFolderButton.position = new Vector2(-fileViewTop.visual[0].scale.X / 2 + 16f, 0f);
            upFolderButton.UpdateHitbox();
            upFolderButton.action = new ActionFolderBack(mc, this);

            int depthOffset = baseRect.depth;
            fileViewRect.depth = depthOffset + 2;
            fileViewRectOutLine.depth = depthOffset + 1;
            fileViewTop.depth = fileViewRect.depth + 1;
            fileViewDecor1.depth = fileViewRect.depth + 1;
            fileViewDecor2.depth = fileViewRect.depth + 1;
            fileViewDecor3.depth = fileViewRect.depth + 1;
            fileViewSide.depth = fileViewRect.depth + 1;

            upFolderButton.depth = fileViewTop.depth + 1;
        }

        private void SetupDecor(Drawrect decor, float width, float height, float xOffset, float lerpAmount)
        {
            decor.visual[0].scale = new Vector2(width, height);
            decor.visual[0].color = Color.Lerp(mc._settings.GetCurrPalletre().boxColor, mc._settings.GetCurrPalletre().baseColor2, lerpAmount);
            decor.position = new Vector2(-fileViewRect.visual[0].scale.X / 2 + xOffset, 16f);
        }

        private Text CreateText(string content)
        {
            Text tx = new Text(null);
            tx.font = _font;
            tx.text = content;
            tx.origin = new Vector2(0f, 0f);
            return tx;
        }

        private TextButton CreateButton(Vector2 position, Text text, string filePath, Action act = null)
        {
            TextButton button = new TextButton(mc, fileViewRect);
            button.action = act;
            button.SetPos(position);
            button.origColor = mc._settings.GetCurrPalletre().textColor2;
            button.effColor = mc._settings.GetCurrPalletre().effectColor2;
            button.SetText(text);
            return button;
        }

        private void ClearLists()
        {
            foreach (TextButton item in dir) item.Destroy();
            foreach (TextButton item in dirType) item.Destroy();
            foreach (TextButton item in dirError) item.Destroy();

            dir.Clear();
            dirType.Clear();
            dirError.Clear();
        }

        private void AddListsToEntityManager()
        {
            foreach (TextButton item in dir) mc._entityManager.AddEntity(item);
            foreach (TextButton item in dirType) mc._entityManager.AddEntity(item);
            foreach (TextButton item in dirError) mc._entityManager.AddEntity(item);
        }
    }
}