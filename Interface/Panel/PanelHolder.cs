using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class PanelHolder : AbstrEntity
    {
        public bool isBase = false;

        public bool isShow = false;

        public Panel panel = null;

        public Drawrect showrect;

        public override AbstrEntity Clone()
        {
            throw new NotImplementedException();
        }

        public Vector2 size = new Vector2(100f,100f);

        public Vector2 origin = new Vector2(0.5f, 0.5f);

        public Rect GetRect()
        {
            return new Rect(position, size, origin);
        }

        public Rect GetPanelPos(Vector2 pos)
        {
            return GetPanelPos(GetPosName(pos));
        }

        public Rect GetPanelPos(string name)
        {
            Rect rect = GetRect();

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

        public String GetPosName(Vector2 pos)
        {
            Rect rect = GetRect();

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

        public void AddPanel(Panel pl, string name)
        {
            if (panel != null) throw new Exception("Не может иметь несколько панелей. Удалите текущую.");

            mc._entityManager.AddEntity(pl);

            panel = pl;

            panel.position = GetPanelPos(name).position - GetPos();
            panel.size = GetPanelPos(name).size;
            panel.baseRect.depth = 1;

            panel.parent = this;
        }

        public void DeletePanel()
        {
            panel = null;
        }

        public override void Update(float deltaTime)
        {
            if (isShow && panel == null)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

                Rect rect = GetPanelPos(mousePosition);

                if (rect != null)
                {
                    showrect.position = TUH.Lerp(showrect.position, rect.position - GetPos(), 0.1f);
                    showrect.visual[0].scale = TUH.Lerp(showrect.visual[0].scale, rect.size, 0.1f);

                    showrect.visual[0].alpha = MathHelper.Lerp(showrect.visual[0].alpha, 0.35f, 0.1f);
                    showrect.visual[0].origin = Vector2.Zero;
                    showrect.visual[0].color =
                        Color.Lerp(
                        mc._settings.GetCurrPalletre().textColor1,
                        mc._settings.GetCurrPalletre().effectColor2,
                        0.5f);

                }
                else
                {
                    showrect.visual[0].alpha = MathHelper.Lerp(showrect.visual[0].alpha, 0f, 0.1f);
                }
            }
        }


        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(showrect);
        }
        public PanelHolder(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            showrect = new Drawrect(mc, this);
        }
    }
}
