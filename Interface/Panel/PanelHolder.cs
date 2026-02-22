using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PanelHolder : AbstrEntity
    {
        public List<Panel> panels = new List<Panel>();

        public List<Rect> map = new List<Rect>();

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException("PanelHolder не поддерживает клонирование");
        }

         
        public Vector2 size = new Vector2(100f,100f);

        public Vector2 origin = new Vector2(0.5f, 0.5f);

        public Rect GetRect()
        {
            return new Rect(position, size, origin);
        }

        public Rect GetPanelPos(Vector2 pos, Rect rect)
        {
            return GetPanelPos(GetPosName(pos, rect), rect);
        }

        public Rect GetPanelPos(string name, Rect rect = null)
        {
            if (rect == null) rect = GetRect();

            if (name == "UpLeft") return rect.GetSubrect(2, 2, 0, 0);
            if (name == "Up") return rect.GetSubrect(1, 2, 0, 0);
            if (name == "UpRight") return rect.GetSubrect(2, 2, 1, 0);

            if (name == "Left") return rect.GetSubrect(2, 1, 0, 0);
            if (name == "Center") return rect;
            if (name == "Right") return rect.GetSubrect(2, 1, 1, 0);

            if (name == "DownLeft") return rect.GetSubrect(2, 2, 0, 1);
            if (name == "Down") return rect.GetSubrect(1, 2, 0, 1);
            if (name == "DownRight") return rect.GetSubrect(2, 2, 1, 1);

            return null;
        }

        public String GetPosName(Vector2 pos, Rect rect = null)
        {
            if (rect == null) rect = GetRect();

            if (rect.GetSubrect(3, 3, 0, 0).CheckPoint(pos)) return "UpLeft";
            if (rect.GetSubrect(3, 3, 1, 0).CheckPoint(pos)) return "Up";
            if (rect.GetSubrect(3, 3, 2, 0).CheckPoint(pos)) return "UpRight";

            if (rect.GetSubrect(3, 3, 0, 1).CheckPoint(pos)) return "Left";
            if (rect.GetSubrect(3, 3, 1, 1).CheckPoint(pos)) return "Center";
            if (rect.GetSubrect(3, 3, 2, 1).CheckPoint(pos)) return "Right";

            if (rect.GetSubrect(3, 3, 0, 2).CheckPoint(pos)) return "DownLeft";
            if (rect.GetSubrect(3, 3, 1, 2).CheckPoint(pos)) return "Down";
            if (rect.GetSubrect(3, 3, 2, 2).CheckPoint(pos)) return "DownRight";

            return "Out";
        }

        public void UpdateCurrMap()
        {
            List<Rect> closedSpaces = new List<Rect>();

            foreach (Panel pl in panels)
            {
                closedSpaces.Add(pl.GetRect());
            }

            map = RectPanelSolver.GetRectMap(GetRect(), closedSpaces);

            Debug.WriteLine(GetRect());
            foreach (Rect rect in map)
            {
                Debug.WriteLine(rect);
            }
        }

        public bool IsSmallRect(Rect rect)
        {
            return (rect.size.X < 100 || rect.size.Y < 100 || rect.size.X * rect.size.Y < 15000);
        }

        public Rect GetRectUnder(Vector2 pos)
        {
            foreach (Rect rect in map)
            {
                if (rect.CheckPoint(pos))
                {
                    if (IsSmallRect(rect))
                    {
                        return rect;
                    }
                    else
                    {
                        return GetPanelPos(pos, rect);
                    }
                }
            }

            return null;
        }

        public void AddPanel(Panel panel, Vector2 pos)
        {
            AddPanel(panel, GetRectUnder(pos));
        }

        public void AddPanel(Panel panel, Rect targetRect)
        {
            if (targetRect == null) throw new NullReferenceException("Целевая позиция отсутствует или не была правильно определена");

            panels.Add(panel);

            panel.targetRect = targetRect;
            panel.lastRect = targetRect;

            panel.parent = this;

            panel.hb = new Hitbox[1];
            panel.hb[0] = new PolygonHitbox(new List<Vector2>
                {
                    new Vector2(position.X, position.Y),
                    new Vector2(position.X + size.X, position.Y),
                    new Vector2(position.X + size.X, position.Y + size.Y),
                    new Vector2(position.X, position.Y + size.Y)
                });
            panel.hb[0].parent = panel;
            panel.hb[0].depth = panel.baseRect.depth;

            UpdateCurrMap();
        }

        public void DeletePanel(Panel panel)
        {
            panels.Remove(panel);

            panel.parent = null;

            UpdateCurrMap();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }


        public override void OnSpawn()
        {
            UpdateCurrMap();
        }

        public PanelHolder(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {

        }
    }
}
