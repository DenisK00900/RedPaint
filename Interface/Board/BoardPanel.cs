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
            texts[4].text = "Анимация";

            float currentX = 30;
            float spacing = 20;

            PopList[] lists = new PopList[5];

            //1

            lists[0] = new PopList(mc, Vector2.Zero);

            Text[] listtext = new Text[4];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Создать";
            listtext[1].text = "Загрузить";
            listtext[2].text = "Сохранить";
            listtext[3].text = "Сохранить как";

            TextButton[] button = new TextButton[4];

            button[0] = new TextButton(mc);
            button[0].AddAction(new ActionSpawn(mc, new DialogCreateNewImage(mc)));
            button[0].AddAction(new ActionDestroy(mc, lists[0]));
            button[0].SetText(listtext[0]);
            lists[0].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].AddAction(new ActionSpawn(mc, new DialogFileLoad(mc)));
            button[1].AddAction(new ActionDestroy(mc, lists[0]));
            button[1].SetText(listtext[1]);
            lists[0].AddMenuElement(button[1]);

            button[2] = new TextButton(mc);
            button[2].SetText(listtext[2]);
            lists[0].AddMenuElement(button[2]);

            button[3] = new TextButton(mc);
            button[3].AddAction(new ActionSpawn(mc, new DialogFileSave(mc)));
            button[3].AddAction(new ActionDestroy(mc, lists[0]));
            button[3].SetText(listtext[3]);
            lists[0].AddMenuElement(button[3]);

            lists[0].AddMenuElement(new DelayMenuElement(mc));

            //2

            lists[1] = new PopList(mc, Vector2.Zero);

            listtext = new Text[3];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Холст";
            listtext[1].text = "Спрайт";
            listtext[2].text = "Настройки";

            button = new TextButton[3];

            button[0] = new TextButton(mc);
            button[0].SetText(listtext[0]);
            lists[1].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].SetText(listtext[1]);
            lists[1].AddMenuElement(button[1]);

            lists[1].AddMenuElement(new DelayMenuElement(mc));

            button[2] = new TextButton(mc);
            button[2].SetText(listtext[2]);
            lists[1].AddMenuElement(button[2]);

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
            button[0].SetText(listtext[0]);
            lists[2].AddMenuElement(button[0]);

            button[1] = new TextButton(mc);
            button[1].AddAction(new ActionPanelHolderDef(mc));
            button[1].AddAction(new ActionDestroy(mc, lists[2]));
            button[1].SetText(listtext[1]);
            lists[2].AddMenuElement(button[1]);

            button[2] = new TextButton(mc);
            button[2].AddAction(new ActionClearPanels(mc));
            button[2].AddAction(new ActionDestroy(mc, lists[2]));
            button[2].SetText(listtext[2]);
            lists[2].AddMenuElement(button[2]);

            lists[2].AddMenuElement(new DelayMenuElement(mc));

            //4-5

            lists[3] = new PopList(mc, Vector2.Zero);
            lists[4] = new PopList(mc, Vector2.Zero);

            baseRect.SetDepth(1);

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i] = new TextButton(mc, this);

                menu[i].SetPos(new Vector2(currentX, 20));
                currentX += texts[i].GetRectSize().X + spacing;

                menu[i].SetText(texts[i]);
                menu[i].AddAction(new ActionSpawn(mc, lists[i], menu[i].depth + 4));

                menu[i].SetHitboxPos(menu[i].GetPos());
                menu[i].SetDepth(baseRect.depth + 1);
            }
        }
    }
}
