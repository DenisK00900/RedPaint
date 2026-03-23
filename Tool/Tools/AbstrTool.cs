using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Reflection.Emit;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Dynamic;

namespace RedPaint
{
    public abstract class AbstrTool : IUseCloneFollows
    {
        public Maincode mc;

        public string name = "None";
        public Texture2D icon;

        public string dest = "Описание инструмента";

        public List<ToolSet> setters = new List<ToolSet>();

        public List<AbstrEntity> follows { get; set; } = new List<AbstrEntity>();

        public AbstrTool (Maincode imc)
        {
            mc = imc;
        }

        public abstract Texture2D GetPrerender(float scale = 1f);

        public virtual Vector2 GetAddPos(float scale = 1f)
        {
            return new Vector2(0, 0);
        }

        public virtual List<ToolSet> GetSets()
        {
            return new List<ToolSet>();
        }

        public Vector2 GetTexPos()
        {
            return mc._image.GetTexPos();
        }

        public T GetValue<T>(string name)
        {
            foreach (ToolSet s in follows)
            {
                if (s.name == name)
                {
                    return s.GetValue<T>();
                }
            }

            throw new Exception("Нет параметра с таким названием");
        }

        public void NewClone(AbstrEntity clone)
        {
            follows.Add(clone);
        }

        public virtual void Update(float deltaTime)
        {

        }
    }
}
