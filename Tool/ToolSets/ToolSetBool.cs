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
    public class ToolSetBool : ToolSet, IDrawable
    {
        public bool value;

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public CheckBox checkbox;

        public override void UpdateHitbox()
        {
            checkbox.UpdateHitbox();
        }

        public override T GetValue<T>()
        {
            return (T)(object)value;
        }

        public override void DetermentPos(Vector2 newpos)
        {
            base.DetermentPos(newpos);

            checkbox.SetPos(new Vector2((visual[0] as Text).GetRectSize().X/2f + 32f,0f));
        }

        public override void Update(float deltaTime)
        {
            (visual[0] as Text).text = name;

            value = checkbox.status;

            base.Update(deltaTime);
        }

        public override Vector2 DetermentSize()
        {
            return (visual[0] as Text).GetRectSize() + new Vector2(0f, 0f);
        }

        public override ToolSetBool Clone()
        {
            ToolSetBool clone = new ToolSetBool(mc, parent);

            clone.value = value;

            clone.depth = depth;

            clone.name = name;

            clone.visual = (this as IDrawable).CloneVisual(clone);

            return clone;
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(checkbox);
        }

        public ToolSetBool(Maincode mc, AbstrEntity pr = null) : base(mc, pr)
        {
            visual = new VisualElement[1];

            visual[0] = new Text(this);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor1, 0.25f);

            name = "Буллевая переменная";

            checkbox = new CheckBox(mc, this);

            checkbox.visual[0].scale = new Vector2(0.5f);
        }
    }
}
