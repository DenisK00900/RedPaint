using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public interface VisualElement
    {
        public IDrawable parent { get; set; }

        public Vector2 pos { get; set; }

        public float rotation { get; set; }

        public Vector2 scale { get; set; }

        public bool isAbsolute { get; set; }

        public Vector2? origin { get; set; }

        public Color color { get; set; }

        public float alpha { get; set; }

        public bool isActive { get; set; }

        public int index { get; set; }

        public virtual void Start()
        {

        }
        public virtual void Destroy()
        {

        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
