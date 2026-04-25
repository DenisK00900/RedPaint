using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace RedPaint
{
    public class DialogAIsettings : DialogBox
    {
        Slider stepNum;
        Slider guidanceScale;

        InputBox seedBox;

        public override DialogAIsettings Clone()
        {
            DialogAIsettings clone = new DialogAIsettings(mc, parent);

            return clone;
        }

        public void OnFinishWrite()
        {
            mc._data.AIsteps = (int)Math.Round(10 + 60 * stepNum.lean);
            mc._data.AIscale = (1.0f + 9.0f * guidanceScale.lean);

            int inputseed;

            try
            {
                inputseed = int.Parse(seedBox.stringInput);
            }
            catch
            {
                inputseed = -1;
            }

            mc._data.AIseed = inputseed > 0 ? inputseed : -1;
        }

        public override void SetDepth(int depth)
        {
            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);
            setRect.SetDepth(depth + 2);

            if (isCreated)
            {
                stepNum.SetDepth(depth + 4);
                guidanceScale.SetDepth(depth + 4);
                seedBox.SetDepth(depth + 4);
            }
        }

        public override void Update(float deltaTime)
        {
            SetDepth(1000);

            (baseRect.visual[4] as Text).text = $"{Math.Round(10 + 60 * stepNum.lean)}";
            (baseRect.visual[5] as Text).text = string.Format(CultureInfo.InvariantCulture, "{0:F1}", 1 + 9 * guidanceScale.lean);

            base.Update(deltaTime);
        }

        public override void OnDrop()
        {
            stepNum.UpdateHitbox();
            guidanceScale.UpdateHitbox();
            seedBox.UpdateHitbox();

            base.OnDrop();
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(340f, 300f);
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            mc._entityManager.AddEntity(stepNum);
            mc._entityManager.AddEntity(guidanceScale);
            mc._entityManager.AddEntity(seedBox);

            OnDrop();
        }

        public DialogAIsettings(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            setRect.headText = "Настройки генерации";

            stepNum = new Slider(mc, baseRect);
            guidanceScale = new Slider(mc, baseRect);
            seedBox = new InputBox(mc, baseRect);

            stepNum.SetPos(120f, 92f);
            guidanceScale.SetPos(120f, 162f);
            seedBox.SetPos(70f, 262f);

            stepNum.SetDef(TUH.InverseLerp(10,70,mc._data.AIsteps));
            guidanceScale.SetDef(TUH.InverseLerp(1.0f, 10.0f, mc._data.AIscale));

            if (mc._data.AIseed != -1)
            {
                seedBox.stringInput = mc._data.AIseed.ToString();
            }

            stepNum.onDrop += OnFinishWrite;
            guidanceScale.onDrop += OnFinishWrite;
            seedBox.onFinishWrite += OnFinishWrite;

            baseRect.visual = new VisualElement[6];

            baseRect.visual[0] = new Sprite(baseRect);
            (baseRect.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");

            baseRect.visual[1] = new Text(baseRect);
            (baseRect.visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[1] as Text).text = "Кол-во шагов";
            (baseRect.visual[1] as Text).origin = Vector2.Zero;
            (baseRect.visual[1] as Text).pos = new Vector2(20f, 48f);

            baseRect.visual[2] = new Text(baseRect);
            (baseRect.visual[2] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[2] as Text).text = "Маштаб";
            (baseRect.visual[2] as Text).origin = Vector2.Zero;
            (baseRect.visual[2] as Text).pos = new Vector2(20f, 118f);

            baseRect.visual[3] = new Text(baseRect);
            (baseRect.visual[3] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[3] as Text).text = "Семя генерации\n(пустое для случайного)";
            (baseRect.visual[3] as Text).origin = Vector2.Zero;
            (baseRect.visual[3] as Text).pos = new Vector2(20f, 192f);

            baseRect.visual[4] = new Text(baseRect);
            (baseRect.visual[4] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[4] as Text).text = "";
            (baseRect.visual[4] as Text).origin = Vector2.Zero;
            (baseRect.visual[4] as Text).pos = new Vector2(220f, 48f);

            baseRect.visual[5] = new Text(baseRect);
            (baseRect.visual[5] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[5] as Text).text = "";
            (baseRect.visual[5] as Text).origin = Vector2.Zero;
            (baseRect.visual[5] as Text).pos = new Vector2(220f, 118f);

            (baseRect.visual[0] as Sprite).scale = DetermentSize() - outlineSize;
            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.85f);
        }
    }
}
