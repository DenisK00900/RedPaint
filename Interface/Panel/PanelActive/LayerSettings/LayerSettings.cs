using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class LayerSettings : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public List<LayerBox> layers = new List<LayerBox>();

        public void AddLayer(Layer lr, Vector2? pos = null)
        {
            LayerBox lb = new LayerBox(mc, lr, this);

            if (pos.HasValue)
            {
                lb.currPos = pos.Value;
                lb.targetPos = pos.Value;
            }

            mc._entityManager.AddEntity(lb);

            layers.Add(lb);
        }

        public void BoxesRepos(int movementIndex)
        {
            if (movementIndex < 0 || movementIndex >= layers.Count || layers.Count <= 1)
                return;

            
        }

        private void UpdateBoxesPos()
        { 
            for (int i = 0; i < layers.Count; i++)
            {
                int visualIndex = layers.Count - 1 - i;

                layers[i].DetermentPos(
                    new Vector2(0f, visualIndex * (32f + layers[i].outlineSize.Y))
                    + new Vector2(0, 32f)
                    + panel.outlineSize / 2f
                );

                layers[i].SetNum(i);

                layers[i].SetDepth(depth + 1);
                layers[i].UpdateHitbox();
            }
        }

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Слой";
            SetDepth(pl.baseRect.depth + 2);
        }

        public override void Update(float deltaTime)
        {
            UpdateBoxesPos();

            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].isTaken)
                {
                    BoxesRepos(i);
                    break;
                }
            }

            base.Update(deltaTime);
        }

        public void UpdateLayers()
        {
            List<Vector2> savedPos = new List<Vector2>();

            int count = layers.Count;

            foreach (LayerBox lb in layers)
            {
                savedPos.Add(lb.currPos);
                lb.Destroy();
            }

            layers.Clear();

            for (int i = 0; i < mc._image.layers.Count; i++)
            {
                if (count == mc._image.layers.Count)
                {
                    AddLayer(mc._image.layers[i], savedPos[i]);
                }
                else
                {
                    AddLayer(mc._image.layers[i]);
                }
            }
        }

        public LayerSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            UpdateLayers();

            mc._image.ChangesLayers += UpdateLayers;
        }
    }
}
