using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public abstract class AbstrEntity
    {
        public Maincode mc { get; set; }

        private AbstrEntity _parent;
        public AbstrEntity parent
        {
            get => _parent;
            set
            {
                if (_parent != null)
                {
                    _parent.children.Remove(this);
                }

                _parent = value;

                if (_parent != null)
                {
                    _parent.children.Add(this);

                    if (this is IDrawable drawableThis && _parent is IDrawable drawableParent)
                    {
                        drawableThis.depth = drawableParent.depth + 1;
                    }
                }
            }
        }

        public List<AbstrEntity> children = new List<AbstrEntity>();

        public Vector2 position;

        public bool markForDestroy = false;

        public bool isAbsolute = false;

        public bool isCreated = false;

        public Vector2 GetPos()
        {
            if (!isAbsolute && parent != null) return parent.GetPos() + position;
            return position;
        }

        public void SetPos(Vector2 pos)
        {
            position = pos;
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void OnSpawn()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void Destroy()
        {
            markForDestroy = true;

            foreach (AbstrEntity ent in children)
            {
                ent.Destroy();
            }
        }

        public abstract AbstrEntity Clone();

        public AbstrEntity(Maincode imc, AbstrEntity pr = null)
        {
            mc = imc;
            position = Vector2.Zero;

            if (pr != null)
            {
                parent = pr;
            }
        }

        public AbstrEntity(Maincode imc, Vector2 pos, AbstrEntity pr = null)
        {
            mc = imc;
            position = pos;

            if (pr != null)
            {
                parent = pr;
            }
        }
    }
}