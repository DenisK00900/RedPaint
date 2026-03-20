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

        public void AddLayer(Layer lr)
        {
            LayerBox lb = new LayerBox(mc, lr, this);

            mc._entityManager.AddEntity(lb);

            layers.Add(lb);
        }

        private void UpdateBoxesPos()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].SetPos(new Vector2(0f, i * (32f + layers[i].outlineSize.Y)) + new Vector2(0,32f) + panel.outlineSize/2f);

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

            base.Update(deltaTime);
        }

        public void UpdateLayers()
        {
            foreach (LayerBox lb in layers)
            {
                lb.Destroy();
            }

            layers.Clear();

            foreach (Layer lr in mc._image.layers)
            {
                AddLayer(lr);
            }
        }

        public LayerSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            UpdateLayers();

            mc._image.ChangesLayers += UpdateLayers;
        }
    }
}
