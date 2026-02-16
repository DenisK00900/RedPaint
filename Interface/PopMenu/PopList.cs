using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                entity.parent = baseRect;

                elements.Add(entity);

                size = DetermentSize();

                entity.SetPos(size / 2f);
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

            clone.elements = elements;

            clone.size = size;
            clone.outlineSize = outlineSize;

            return clone;
        }

        public Vector2 DetermentSize()
        {
            if (elements.Count == 0) return new Vector2(96, 64);

            float w = 0; 
            float h = 0;

            foreach (AbstrEntity item in elements)
            {
                Vector2 itemSize = (item as IMenuElement).GetSize();

                if (itemSize.X > w)
                {
                    w = itemSize.X;
                }

                h += itemSize.Y + 4;
            }

            return new Vector2(w + 8f, h + 8f);
        }

        public override void OnSpawn()
        {
            size = DetermentSize();

            visual[0].pos = size / 2f;

            visual[0].isActive = elements.Count == 0;

            base.OnSpawn();

            foreach (AbstrEntity item in elements)
            {
                mc._entityManager.AddEntity(item);
            }
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

            visual[0] = new Text(this);

            (visual[0] as Text).text = "Список\nпуст";
            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).color =
            Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.75f);

            visual[0].pos = size / 2f;

            depth = baseRect.depth + 2;
        }
    }
}
