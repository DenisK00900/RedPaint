using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int GetTakenIndex()
        {
            int takenIndex = -1;

            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i].isTaken)
                {
                    takenIndex = i;
                    break;
                }
            }

            return takenIndex;
        }

        private int GetClosestIndex(Vector2 pos)
        {
            int closestIndex = -1;
            float minDist = float.MaxValue;

            for (int i = 0; i < layers.Count; i++)
            {
                int visualIndex = layers.Count - 1 - i;

                float pos1 = (mc._input.GetMousePosition() - activeRect.position).Y + panel.outlineSize.Y / 2f;

                float pos2 = visualIndex * (32f + layers[i].outlineSize.Y) + 32f + panel.outlineSize.Y / 2f;

                float dist = Math.Abs(pos1 - pos2);

                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        private void UpdateBoxesPos()
        {
            int takenIndex = GetTakenIndex();

            for (int i = 0; i < layers.Count; i++)
            {
                int visualIndex = layers.Count - 1 - i;

                if (takenIndex == i)
                {
                    layers[i].DetermentPos(
                            new Vector2(0f, (mc._input.GetMousePosition() - activeRect.position).Y)
                            + panel.outlineSize / 2f
                        );

                    layers[i].SetDepth(depth + 7);

                    layers[i].SetNum(i);

                    layers[i].canGlow = true;
                }
                else
                {
                    layers[i].DetermentPos(
                            new Vector2(0f, visualIndex * (32f + layers[i].outlineSize.Y))
                            + new Vector2(0, 32f)
                            + panel.outlineSize / 2f
                        );

                    layers[i].SetDepth(depth + 1);

                    layers[i].canGlow = false;
                }

                if (takenIndex == -1)
                {
                    layers[i].SetNum(i);
                    layers[i].canGlow = true;
                }

                layers[i].UpdateHitbox();
            }
        }

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Слои";
            SetDepth(pl.baseRect.depth + 2);
        }

        public void UpdateMovement()
        {
            int takenIndex = GetTakenIndex();

            if (takenIndex == -1) return;

            TUH.MoveItem(layers, takenIndex, 
                GetClosestIndex(new Vector2(0f, (mc._input.GetMousePosition() - activeRect.position).Y)
                            + panel.outlineSize / 2f));

            layers[takenIndex].SetThisLayer();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            UpdateMovement();

            UpdateBoxesPos();
        }

        public void AddLayer(Layer lr)
        {
            LayerBox lb = new LayerBox(mc, lr, this);

            mc._entityManager.AddEntity(lb);

            layers.Add(lb);
        }

        public void RemoveLayer(Layer lr)
        {
            foreach (LayerBox lb in layers)
            {
                if (lb.layer == lr)
                {
                    lb.Destroy();
                    layers.Remove(lb);

                    break;
                }
            }
        }

        public void UpdateLayersList()
        {
            var managerLayers = mc._image.layers;

            foreach (var managerLayer in managerLayers)
            {
                bool found = false;
                foreach (var layerBox in layers)
                {
                    if (layerBox.layer == managerLayer)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    AddLayer(managerLayer);
                }
            }

            var toRemove = new List<LayerBox>();
            foreach (var layerBox in layers)
            {
                bool found = false;
                foreach (var managerLayer in managerLayers)
                {
                    if (layerBox.layer == managerLayer)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    toRemove.Add(layerBox);
                }
            }

            foreach (var lb in toRemove)
            {
                lb.Destroy();
                layers.Remove(lb);
            }

            var reorderedBoxes = new List<LayerBox>();
            foreach (var managerLayer in managerLayers)
            {
                foreach (var layerBox in layers)
                {
                    if (layerBox.layer == managerLayer)
                    {
                        reorderedBoxes.Add(layerBox);
                        break;
                    }
                }
            }
            layers.Clear();
            layers.AddRange(reorderedBoxes);
        }

        public void SetLayers()
        {
            var reorderedLayers = new List<Layer>();
            foreach (var layerBox in layers)
            {
                reorderedLayers.Add(layerBox.layer);
            }

            Layer workingLayerRef = null;
            if (mc._image.workingLayer >= 0 && mc._image.workingLayer < mc._image.layers.Count)
            {
                workingLayerRef = mc._image.layers[mc._image.workingLayer];
            }

            mc._image.layers = reorderedLayers;

            if (workingLayerRef != null)
            {
                int newIndex = mc._image.layers.IndexOf(workingLayerRef);
                mc._image.workingLayer = newIndex >= 0 ? newIndex : 0;
            }
            else
            {
                mc._image.workingLayer = 0;
            }

            mc._image.CallChangesLayers();
        }

        public LayerSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            mc._image.ChangesLayers += UpdateLayersList;
        }
    }
}
