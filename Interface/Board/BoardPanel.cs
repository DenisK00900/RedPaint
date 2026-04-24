    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Runtime;
    using System.Text;

    namespace RedPaint
    {
        public class BoardPanel : AbstrEntity
        {
            Drawrect baseRect;
            TextButton[] menu;

            public override void OnSpawn()
            {
                for (int i = 0; i < menu.Length; i++)
                {
                    mc._entityManager.AddEntity(menu[i]);
                }
            }

            public override BoardPanel Clone()
            {
                BoardPanel clone = new BoardPanel(mc);

                clone.SetPos(position);
                foreach (AbstrEntity item in children)
                {
                    clone.children.Add(item.Clone());
                }

                return clone;
            }

        public BoardPanel(Maincode mc) : base(mc)
        {
            baseRect = new Drawrect(mc, this);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color = mc._settings.GetCurrPalletre().baseColor2;
            (baseRect.visual[0] as Sprite).scale = new Vector2(mc._data.res.X, 60);
            SetPos(Vector2.Zero);
            mc._entityManager.AddEntity(baseRect);

            Text[] texts = new Text[5];
            menu = new TextButton[5];
            SpriteFont font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");

            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = new Text(null);
                texts[i].font = font;
                texts[i].origin = new Vector2(0, 0.5f);
            }

            texts[0].text = "Файл";
            texts[1].text = "Изменить";
            texts[2].text = "Вид";
            texts[3].text = "Слой";
            texts[4].text = "ИИ";

            float currentX = 30;
            float spacing = 20;

            PopList[] lists = new PopList[5];

            //1

            lists[0] = new PopList(mc, Vector2.Zero);

            Text[] listtext = new Text[5];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Создать";
            listtext[1].text = "Загрузить";
            listtext[2].text = "Сохранить";
            listtext[3].text = "Сохранить как";
            listtext[4].text = "Недавние";

            TextButton[] button = new TextButton[5];

            button[0] = new TextButton(mc);
            button[0].AddAction(new ActionSpawn(mc, new DialogCreateNewImage(mc)));
            button[0].AddAction(new ActionDestroy(mc, lists[0]));
            button[0].SetHintText("Создать пустое изображение\nзаданного размера\n\nCtrl+N");
            button[0].SetText(listtext[0]);
            lists[0].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].AddAction(new ActionSpawn(mc, new DialogFileLoad(mc)));
            button[1].AddAction(new ActionDestroy(mc, lists[0]));
            button[1].SetHintText("Выбрать и загрузить\nизображение из проводника\n\nCtrl+O");
            button[1].SetText(listtext[1]);
            lists[0].AddMenuElement(button[1]);

            button[2] = new TextButton(mc);
            button[2].SetText(listtext[2]);
            button[2].SetHintText("Сохранить изображение как\nпоследнее открытое\n\nCtrl+S");
            lists[0].AddMenuElement(button[2]);

            button[3] = new TextButton(mc);
            button[3].AddAction(new ActionSpawn(mc, new DialogFileSave(mc)));
            button[3].AddAction(new ActionDestroy(mc, lists[0]));
            button[3].SetHintText("Выбрать как сохранить\nизображение в проводнике\n\nCtrl+Shift+S");
            button[3].SetText(listtext[3]);
            lists[0].AddMenuElement(button[3]);

            lists[0].AddMenuElement(new DelayMenuElement(mc));

            button[4] = new TextButton(mc);
            button[4].SetHintText("Открыть один из последних\nизменённых файлов");
            button[4].SetText(listtext[4]);
            lists[0].AddMenuElement(button[4]);

            //2

            PopList rotateList = new PopList(mc, Vector2.Zero);

            List<TextButton> rotateListButtons = new List<TextButton>();

            Text[] rotateListButtonsText = new Text[3];

            for (int i = 0; i < rotateListButtonsText.Length; i++)
            {
                rotateListButtonsText[i] = new Text(null);
                rotateListButtonsText[i].font = font;

                rotateListButtons.Add(new TextButton(mc));
            }

            rotateListButtonsText[0].text = "90 По часовой";
            rotateListButtonsText[1].text = "90 Против часовой";
            rotateListButtonsText[2].text = "Поворот на 180";

            rotateListButtons[0] = new TextButton(mc);
            rotateListButtons[0].SetText(rotateListButtonsText[0]);
            rotateListButtons[0].AddAction(new ActionDestroy(mc, rotateList));
            rotateListButtons[0].AddAction(new ActionRotateImage(mc, 1));
            rotateListButtons[0].SetHintText("Повернуть изображение на 90 градусов по часовой стрелке");
            rotateList.AddMenuElement(rotateListButtons[0]);

            rotateListButtons[1] = new TextButton(mc);
            rotateListButtons[1].SetText(rotateListButtonsText[1]);
            rotateListButtons[1].AddAction(new ActionDestroy(mc, rotateList));
            rotateListButtons[1].AddAction(new ActionRotateImage(mc, 3));
            rotateListButtons[1].SetHintText("Повернуть изображение на 90 градусов против часовой стрелки");
            rotateList.AddMenuElement(rotateListButtons[1]);

            rotateListButtons[2] = new TextButton(mc);
            rotateListButtons[2].SetText(rotateListButtonsText[2]);
            rotateListButtons[2].AddAction(new ActionDestroy(mc, rotateList));
            rotateListButtons[2].AddAction(new ActionRotateImage(mc, 2));
            rotateListButtons[2].SetHintText("Повернуть изображение на 180 градусов");
            rotateList.AddMenuElement(rotateListButtons[2]);

            lists[1] = new PopList(mc, Vector2.Zero);

            listtext = new Text[7];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Откат";
            listtext[1].text = "Вперёд";
            listtext[2].text = "Холст";
            listtext[3].text = "Спрайт";
            listtext[4].text = "Вращать";
            listtext[5].text = "Отразить";
            listtext[6].text = "Настройки";

            button = new TextButton[7];

            button[0] = new TextButton(mc);
            button[0].SetText(listtext[0]);
            button[0].SetHintText("Откатить последнее изменение\n\nCtrl+Z");
            lists[1].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].SetText(listtext[1]);
            button[1].SetHintText("Вернуть последнее изменение\n\nCtrl+Y");
            lists[1].AddMenuElement(button[1]);

            lists[1].AddMenuElement(new DelayMenuElement(mc));

            button[2] = new TextButton(mc);
            button[2].SetText(listtext[2]);
            button[2].SetHintText("Изменить холст");
            lists[1].AddMenuElement(button[2]);

            button[3] = new TextButton(mc);
            button[3].SetText(listtext[3]);
            button[3].SetHintText("Изменить изображение");
            lists[1].AddMenuElement(button[3]);

            lists[1].AddMenuElement(new DelayMenuElement(mc));

            button[4] = new TextButton(mc);
            button[4].SetText(listtext[4]);
            button[4].SetHintText("Повернуть изображение");
            button[4].AddAction(new ActionDestroy(mc, lists[1]));
            button[4].AddAction(new ActionSpawn(mc, rotateList));
            lists[1].AddMenuElement(button[4]);

            button[5] = new TextButton(mc);
            button[5].SetText(listtext[5]);
            button[5].SetHintText("Отразить изображение");
            lists[1].AddMenuElement(button[5]);

            lists[1].AddMenuElement(new DelayMenuElement(mc));

            button[6] = new TextButton(mc);
            button[6].SetText(listtext[6]);
            button[6].SetHintText("Открыть настройки приложения\n\nCtrl+K");
            lists[1].AddMenuElement(button[6]);

            //3

            lists[2] = new PopList(mc, Vector2.Zero);

            listtext = new Text[3];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Новая панель";
            listtext[1].text = "Восстановить";
            listtext[2].text = "Очистить";

            button = new TextButton[3];

            button[0] = new TextButton(mc);
            button[0].AddAction(new ActionNewPanel(mc));
            button[0].AddAction(new ActionDestroy(mc, lists[2]));
            button[0].SetHintText("Создать новую пустую\nпанель");
            button[0].SetText(listtext[0]);
            lists[2].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].AddAction(new ActionPanelHolderDef(mc));
            button[1].AddAction(new ActionDestroy(mc, lists[2]));
            button[1].SetHintText("Восстановить исходное\nсостояние панелей");
            button[1].SetText(listtext[1]);
            lists[2].AddMenuElement(button[1]);

            button[2] = new TextButton(mc);
            button[2].AddAction(new ActionClearPanels(mc));
            button[2].AddAction(new ActionDestroy(mc, lists[2]));
            button[2].SetHintText("Удалить все панели");
            button[2].SetText(listtext[2]);
            lists[2].AddMenuElement(button[2]);

            lists[2].AddMenuElement(new DelayMenuElement(mc));

            //4

            lists[3] = new PopList(mc, Vector2.Zero);

            listtext = new Text[2];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Новый слой";
            listtext[1].text = "Соединить";

            button = new TextButton[2];

            button[0] = new TextButton(mc);
            button[0].AddAction(new ActionNewLayer(mc));
            button[0].AddAction(new ActionDestroy(mc, lists[3]));
            button[0].SetHintText("Добавить новый пустой слой");
            button[0].SetText(listtext[0]);
            lists[3].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].SetHintText("Соединить все слои в один слой");
            button[1].SetText(listtext[1]);
            lists[3].AddMenuElement(button[1]);

            //5

            lists[4] = new PopList(mc, Vector2.Zero);

            baseRect.SetDepth(1);

            string[] menuHints = new string[5];
            menuHints[0] = "Управление файлами";
            menuHints[1] = "Настройки текущего изображения\nи приложения";
            menuHints[2] = "Настройка вида приложения и панелей";
            menuHints[3] = "Настройка слоёв";
            menuHints[4] = "Настройка анимации";

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i] = new TextButton(mc, this);

                menu[i].SetPos(new Vector2(currentX, 20));
                currentX += texts[i].GetRectSize().X + spacing;

                menu[i].SetHintText(menuHints[i]);
                menu[i].SetText(texts[i]);
                menu[i].AddAction(new ActionSpawn(mc, lists[i], menu[i].depth + 4));

                menu[i].SetHitboxPos(menu[i].GetPos());
                menu[i].SetDepth(baseRect.depth + 1);
            }
        }
    }
}
