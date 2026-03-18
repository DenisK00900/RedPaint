using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ToolSettings : PanelActive, IDrawable
    {
        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        private AbstrTool tool = null;

        public override void SetPanel(Panel pl)
        {
            base.SetPanel(pl);
            pl.setRect.headText = "Настройка";
            depth = pl.baseRect.depth + 2;
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            tool = mc._image.currTool;

            SetPos(new Vector2(0f,32f) + panel.outlineSize / 2f);
            visual[0].scale = new Vector2(activeRect.size.X - panel.outlineSize.X, 32f);

            SetPos(new Vector2(0f, 32f) + panel.outlineSize / 2f);

            (visual[1] as Text).text = tool.name;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
        }

        public ToolSettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            visual = new VisualElement[2];

            visual[0] = new Sprite(this);
            (visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");

            visual[0].origin = Vector2.Zero;
            visual[0].color =
                Color.Lerp(mc._settings.GetCurrPalletre().baseColor1, mc._settings.GetCurrPalletre().baseColor2, 0.9f);

            visual[1] = new Text(this);
            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[1] as Text).text = "Название инструмента";
            visual[1].origin = Vector2.Zero;
            visual[1].pos = new Vector2(8f, 8f);
            visual[1].color =
                Color.Lerp(mc._settings.GetCurrPalletre().textColor1, mc._settings.GetCurrPalletre().baseColor2, 0.5f);
        }
    }
}
