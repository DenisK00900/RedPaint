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
            TextExpMenu menu1;

            public override void OnSpawn()
            {
                mc._entityManager.AddEntity(menu1);
            }

            public override BoardPanel Clone()
            {
                BoardPanel clone = new BoardPanel(mc);

                clone.SetPos(GetPos());
                foreach (AbstrEntity item in children)
                {
                    clone.children.Add(item.Clone());
                }

                //clone.baseRect = baseRect.Clone() as Drawrect; Избегать двойного клонирования!!!
                //clone.menu1 = menu1.Clone() as TextExpMenu;Избегать двойного клонирования!!!

                return clone;
            }

            public BoardPanel(Maincode mc) : base(mc)
            {
                baseRect = new Drawrect(mc);

                (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
                (baseRect.visual[0] as Sprite).color = mc._settings.GetCurrPalletre().baseColor2;
                (baseRect.visual[0] as Sprite).scale = new Vector2(mc._data.res.X, 60);

                SetPos(Vector2.Zero);

                mc._entityManager.AddEntity(baseRect);

                Text text1 = new Text(null);

                text1.font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
                text1.text = "Файл";

                menu1 = new TextExpMenu(mc, text1, new PopMenu(mc, Vector2.Zero));

                menu1.SetPos(new Vector2(50, 30));
            }
        }
    }
