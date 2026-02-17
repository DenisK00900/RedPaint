using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RedPaint
{
    public class PopList : PopMenu, IDrawable
    {
        private List<AbstrEntity> elements = new List<AbstrEntity>();

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public void AddMenuElement(AbstrEntity entity)
        {
            if (entity is IMenuElement)
            {
                elements.Add(entity);
            }
            else
            {
                throw new Exception("Элемент должен быть интерфейсом IMenuElement");
            }
        }

        public void RemoveMenuElement(AbstrEntity entity)
        {
            elements.Remove(entity);
        }

        public override PopList Clone()
        {
            PopList clone = new PopList(mc, position, parent);

            foreach (AbstrEntity item in children)
            {
                clone.children.Add(item.Clone());
            }

            clone.visual = ((IDrawable)this).CloneVisual();

            clone.elements = new List<AbstrEntity>();

            foreach (AbstrEntity item in elements)
            {
                clone.elements.Add(item.Clone());
            }

            clone.size = size;
            clone.outlineSize = outlineSize;

            return clone;
        }

        public Vector2 DetermentSize()
        {
            if (elements.Count == 0) return new Vector2(96, 64);

            float w = 0; 
            float h = 4;

            foreach (AbstrEntity item in elements)
            {
                Vector2 itemSize = (item as IMenuElement).GetSize();

                if (itemSize.X > w)
                {
                    w = itemSize.X + 4;
                }

                h += itemSize.Y + 4;
            }

            return new Vector2(w + 8f, h + 8f);
        }

        public override void OnSpawn()
        {
            size = DetermentSize();

            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            SetPos(mousePosition);

            visual[0].pos = GetPos() + size/2f;

            visual[0].isActive = elements.Count == 0;

            base.OnSpawn();

            depth = baseRect.depth + 2;

            float Ypos = -4;

            foreach (AbstrEntity item in elements)
            {
                Ypos = Ypos + 4 + (item as IMenuElement).GetSize().Y;

                (item as IMenuElement).SetElementPos(new Vector2(size.X/2f, Ypos));

                item.parent = this;

                mc._entityManager.AddEntity(item);
            }
        }

        public override void Update(float deltaTime)
        {
            foreach (AbstrEntity item in elements)
            {
                item.Update(deltaTime);
            }

            base.Update(deltaTime);
        }

        public override void OnDestroy()
        {
            foreach (AbstrEntity item in elements)
            {
                item.Destroy();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (VisualElement item in visual)
            {
                item.Draw(sb);
            }
        }

        public PopList(Maincode mc, Vector2 pos, AbstrEntity pr = null) : base(mc, pos, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Text(baseRect);

            (visual[0] as Text).text = "Список\nпуст";
            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).color =
            Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.75f);
        }
    }
}
