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
            TextExpMenu[] menu;

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
            baseRect = new Drawrect(mc);

            baseRect.parent = this;

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color = mc._settings.GetCurrPalletre().baseColor2;
            (baseRect.visual[0] as Sprite).scale = new Vector2(mc._data.res.X, 60);
            SetPos(Vector2.Zero);
            mc._entityManager.AddEntity(baseRect);

            Text[] texts = new Text[5];
            menu = new TextExpMenu[5];
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

            lists[0] = new PopList(mc, Vector2.Zero);

            Text[] listtext = new Text[3];
            for (int i = 0; i < listtext.Length; i++)
            {
                listtext[i] = new Text(null);
                listtext[i].font = font;
            }

            listtext[0].text = "Создать";
            listtext[1].text = "Загрузить";
            listtext[2].text = "Сохранить";

            for (int i = 0; i < listtext.Length; i++)
            {
                TextExpMenu el = new TextExpMenu(mc, listtext[i], null);

                lists[0].AddMenuElement(el);
            }

            lists[1] = new PopList(mc, Vector2.Zero);
            lists[2] = new PopList(mc, Vector2.Zero);
            lists[3] = new PopList(mc, Vector2.Zero);
            lists[4] = new PopList(mc, Vector2.Zero);

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i] = new TextExpMenu(mc, texts[i], lists[i]);
                menu[i].parent = this;
                menu[i].SetPos(new Vector2(currentX, 20));
                currentX += texts[i].GetRectSize().X + spacing;
            }
        }
    }
}
