using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace RedPaint
{
    public class DialogFileSave : DialogBox, IFileBrowser
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
        public string currDir { get; set; } = "";

        public List<TextButton> dir = new List<TextButton>();
        public List<TextButton> dirType = new List<TextButton>();
        public List<TextButton> dirError = new List<TextButton>();

        private SpriteFont _font;

        public InputBox InputFileName;

        public SaveImageButton saveButton;
        public ActionSaveImage saveAct;

        public override DialogFileSave Clone()
        {
            DialogFileSave clone = new DialogFileSave(mc, parent);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(1000f, 850f);
        }

        public override void OnSpawn()
        {
            if (mc._image.GetCurrentImage() == null)
            {
                mc._entityManager.AddEntity(
                    new DialogMessage(
                        mc,
                        "Ошибка сохранения",
                        "Нет изображения для сохранения",
                        null
                        ));

                Destroy();

                return;
            }

            mc._entityManager.AddEntity(fileViewRect);
            mc._entityManager.AddEntity(fileViewRectOutLine);
            mc._entityManager.AddEntity(fileViewTop);
            mc._entityManager.AddEntity(fileViewDecor1);
            mc._entityManager.AddEntity(fileViewDecor2);
            mc._entityManager.AddEntity(fileViewDecor3);
            mc._entityManager.AddEntity(fileViewSide);

            mc._entityManager.AddEntity(upFolderButton);

            mc._entityManager.AddEntity(InputFileName);

            mc._entityManager.AddEntity(saveButton);

            UpdateListInfo();
            base.OnSpawn();
        }

        public void FolderUp()
        {
            currDir = FileBrowserSolver.GetParentDirectory(currDir);

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

                AbstrAction btnNameAct;
                if (FileBrowserSolver.GetTypeOfPath(cont) == "Изобр.")
                {
                    btnNameAct = null;
                }
                else
                {
                    btnNameAct = new ActionFileBrowser(mc, this, cont);
                }

                TextButton btnName = CreateButton(posName, textName, cont, btnNameAct);

                TextButton btnType = CreateButton(posType, textType, cont);
                TextButton btnStatus = CreateButton(posStatus, textStatus, cont);

                dir.Add(btnName);
                dirType.Add(btnType);
                dirError.Add(btnStatus);

                currentY += itemHeight;
            }

            AddListsToEntityManager();
        }

        public override void OnDrop()
        {
            upFolderButton.UpdateHitbox();

            UpdateListInfo();

            saveButton.UpdateHitbox();
            InputFileName.UpdateHitbox();
        }

        public override void SetDepth(int depth)
        {
            depth += 10;

            fileViewRect.SetDepth(depth + 2);
            fileViewRectOutLine.SetDepth(depth + 1);
            fileViewTop.SetDepth(depth + 3);
            fileViewDecor1.SetDepth(depth + 3);
            fileViewDecor2.SetDepth(depth + 3);
            fileViewDecor3.SetDepth(depth + 3);
            fileViewSide.SetDepth(depth + 3);

            upFolderButton.SetDepth(depth + 4);

            InputFileName.SetDepth(depth + 2);

            saveButton.SetDepth(depth + 2);

            base.SetDepth(depth);
        }

        public DialogFileSave(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            setRect.headText = "Сохранить как";

            _font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");

            fileViewRect = new Drawrect(mc, baseRect);
            fileViewRectOutLine = new Drawrect(mc, fileViewRect);

            Vector2 size = DetermentSize();
            Vector2 innerSize = size - new Vector2(40f, 72f) - new Vector2(0f,80f);

            fileViewRect.visual[0].scale = innerSize;
            fileViewRect.visual[0].color = mc._settings.GetCurrPalletre().boxColor;
            fileViewRect.position = size / 2f - outlineSize / 2f + new Vector2(0f, 16f) - new Vector2(0f, 40f);

            fileViewTop = new Drawrect(mc, fileViewRect);
            fileViewDecor1 = new Drawrect(mc, fileViewRect);
            fileViewDecor2 = new Drawrect(mc, fileViewRect);
            fileViewDecor3 = new Drawrect(mc, fileViewRect);
            fileViewSide = new Drawrect(mc, fileViewRect);
            upFolderButton = new FileLoadUpFolderButton(mc, fileViewTop);

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
            upFolderButton.AddAction(new ActionFolderBack(mc, this));

            InputFileName = new InputBox(mc, baseRect);
            InputFileName.SetSize(256f);
            InputFileName.SetPos(new Vector2(20f + InputFileName.DetermentSize().X/2f, size.Y-40f));
            InputFileName.includeNum = true;
            InputFileName.includeAlp = true;

            saveAct = new ActionSaveImage(mc, this);

            saveButton = new SaveImageButton(mc, baseRect);
            saveButton.AddAction(saveAct);
            saveButton.AddAction(new ActionDestroy(mc, this));
            saveButton.SetPos(new Vector2(size.X - saveButton.visual[0].scale.X / 2f - 20f, size.Y - 40f));

            SetDepth(4);
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

        private TextButton CreateButton(Vector2 position, Text text, string filePath, AbstrAction act = null)
        {
            TextButton button = new TextButton(mc, fileViewRect);
            button.AddAction(act);
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