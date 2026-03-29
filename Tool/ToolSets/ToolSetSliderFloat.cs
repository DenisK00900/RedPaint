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
using System.Diagnostics;
using System.ComponentModel;

namespace RedPaint
{
    public class ToolSetSliderFloat : ToolSet, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        Slider slider;

        public override ToolSetSliderFloat Clone()
        {
            ToolSetSliderFloat clone = new ToolSetSliderFloat(mc, parent);

            clone.depth = depth;

            clone.name = name;

            clone.visual = (this as IDrawable).CloneVisual(clone);

            return clone;
        }

        public override void Update(float deltaTime)
        {
            (visual[0] as Text).text = name;

            (visual[1] as Text).text = $"{slider.lean}";

            base.Update(deltaTime);
        }

        public void SetDef(float value)
        {
            slider.SetDef(value);
        }

        public override void DetermentPos(Vector2 newpos)
        {
            base.DetermentPos(newpos);

            slider.SetPos(new Vector2(-(visual[0] as Text).GetRectSize().X / 2 + (slider.leight / 2f + 8f), 32f));

            visual[1].pos = new Vector2(-(visual[0] as Text).GetRectSize().X / 2 + (slider.leight + 8f) + (visual[1] as Text).GetRectSize().X + 16f, 32f);
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(Math.Min(slider.leight, (visual[0] as Text).GetRectSize().X),
                (visual[0] as Text).GetRectSize().Y);
        }

        public override Vector2 DetermentOffset()
        {
            return DetermentSize() + new Vector2(0f, 56f);
        }

        public override T GetValue<T>()
        {
            return (T)(object)(slider.lean);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(slider);

            base.OnSpawn();
        }

        public override void SetDepth(int depth)
        {
            slider.SetDepth(depth + 1);

            base.SetDepth(depth);
        }

        public override void UpdateHitbox()
        {
            slider.UpdateHitbox();
        }

        public ToolSetSliderFloat(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            visual[1] = new Text(this);

            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            slider = new Slider(mc, this);

            slider.SetDef(0f);

            name = "Дробный слайдер";
        }
    }
}
