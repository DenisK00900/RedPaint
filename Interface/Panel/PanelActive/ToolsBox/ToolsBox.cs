using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RedPaint
{
    public class ToolsBox : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public List<ToolsRegion> regions = new List<ToolsRegion>();    

        public Vector2 GetPosForRegion(int num = 0)
        {
            int posY = 32;

            for (int i = 0; i < num + 1; i++)
            {
                posY += (int)regions[i].DetermentSize().Y;
            }

            return 
                new Vector2(regions[num].DetermentSize().X/2f, posY - regions[num].DetermentSize().Y/2f) +
                regions[num].outlineSize/2f;
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < regions.Count(); i++)
            {
                regions[i].SetPos(GetPosForRegion(i));
            }

            base.Update(deltaTime);
        }

        public void AddRegion(ToolsRegion reg)
        {
            if (!isCreated) throw new InvalidOperationException("ToolsBox должен быть добавлен как объект перед настройкой");

            reg.parent = this;
            reg.SetDepth(depth + 1);
            regions.Add(reg);
            mc._entityManager.AddEntity(reg);
        }

        public ToolsBox(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
           
        }
        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Инструменты";
            depth = pl.baseRect.depth + 2;
        }

        public override void OnSpawn()
        {
            ToolsBoxSettings.SetTools(this);
        }
    }
}
