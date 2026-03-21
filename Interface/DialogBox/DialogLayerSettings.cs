using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RedPaint
{
    public class DialogLayerSettings : DialogBox, IDrawable
    {
        public LayerBox box;

        public InputBox nameBox;

        public Slider alphaSlider;

        public VisualElement[] visual { get; set; }
        public int depth { get; set; }

        public override DialogLayerSettings Clone()
        {
            DialogLayerSettings clone = new DialogLayerSettings(mc, box, parent);

            return clone;
        }

        public override void SetDepth(int depth)
        {
            base.SetDepth(depth + 3);

            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);
            setRect.SetDepth(depth + 2);

            if (isCreated)
            {
                nameBox.SetDepth(depth + 3);
                alphaSlider.SetDepth(depth + 3);
            }
        }

        public override void Update(float deltaTime)
        {
            SetDepth(1000);

            (visual[1] as Text).text = (alphaSlider.lean).ToString("F2");

            box.layer.alpha = alphaSlider.lean;

            base.Update(deltaTime);
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(300,200);
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            mc._entityManager.AddEntity(nameBox);

            mc._entityManager.AddEntity(alphaSlider);
        }

        public void SetName()
        {
            box.SetNewName(nameBox.stringInput);
        }

        public DialogLayerSettings(Maincode imc, LayerBox lb, AbstrEntity pr = null) : base(imc, pr)
        {
            box = lb;

            setRect.headText = "Настройки слоя";

            nameBox = new InputBox(mc, baseRect);

            nameBox.SetPos(new Vector2(DetermentSize().X / 2f, 64f));

            nameBox.stringInput = lb.layer.name;

            nameBox.includeAlp = true;

            nameBox.SetSize(256);

            alphaSlider = new Slider(mc, baseRect);

            alphaSlider.SetPos(new Vector2(DetermentSize().X / 2f - 32f, 160f));

            alphaSlider.SetDef(lb.layer.alpha);

            nameBox.onFinishWrite += SetName;

            visual = new VisualElement[2];

            visual[0] = new Text(baseRect);

            (visual[0] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[0] as Text).text = "Прозрачность";
            visual[0].color = mc._settings.GetCurrPalletre().textColor1;

            visual[0].pos = new Vector2(DetermentSize().X / 2f - 32f, 128f);

            visual[1] = new Text(baseRect);

            (visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (visual[1] as Text).text = "1.00";
            visual[1].color = mc._settings.GetCurrPalletre().textColor1;

            visual[1].pos = new Vector2(DetermentSize().X - 40f, 160f);
        }
    }
}
