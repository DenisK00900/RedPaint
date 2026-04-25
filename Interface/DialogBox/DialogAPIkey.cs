using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RedPaint
{
    public class DialogAPIkey : DialogBox
    {
        InputBox keyinput;

        CheckBox isUseStandartKey;

        public override DialogAPIkey Clone()
        {
            DialogAPIkey clone = new DialogAPIkey(mc, parent);

            return clone;
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(700f, 160f);
        }

        public override void SetDepth(int depth)
        {
            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);
            setRect.SetDepth(depth + 2);

            if (isCreated)
            {
                keyinput.SetDepth(depth + 4);
            }
        }

        public override void Update(float deltaTime)
        {
            SetDepth(1000);

            base.Update(deltaTime);
        }

        public void OnFinishWrite()
        {
            mc._data.userKey = keyinput.stringInput;
        }

        public override void OnDrop()
        {
            keyinput.UpdateHitbox();
            isUseStandartKey.UpdateHitbox();

            base.OnDrop();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            mc._entityManager.AddEntity(keyinput);
            mc._entityManager.AddEntity(isUseStandartKey);

            keyinput.UpdateHitbox();
            isUseStandartKey.UpdateHitbox();
        }

        public DialogAPIkey(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            setRect.headText = "Ключ";

            keyinput = new InputBox(mc, baseRect);

            keyinput.includeAlp = true;

            keyinput.stringInput = mc._data.userKey;

            keyinput.SetSize(580f);
            keyinput.SetPos(310f, 72f);

            keyinput.onFinishWrite += OnFinishWrite;

            isUseStandartKey = new CheckBox(mc, baseRect);

            isUseStandartKey.status = mc._data.useStandratKey;
            isUseStandartKey.ChangeIcon();

            isUseStandartKey.onAction.Add(new ActionToggleAPIkey(mc, true));
            isUseStandartKey.offAction.Add(new ActionToggleAPIkey(mc, false));

            isUseStandartKey.visual[0].scale = new Vector2(0.5f);
            isUseStandartKey.SetPos(36f, 122f);
            isUseStandartKey.SetHintText("Использовать встроенный ключ\nвместо пользовательского.\nМожет быть неактуален.");

            baseRect.visual = new VisualElement[2];

            baseRect.visual[0] = new Sprite(baseRect);
            (baseRect.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");

            baseRect.visual[1] = new Text(baseRect);
            (baseRect.visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[1] as Text).text = $"Встроенный ключ (актуален на {mc._data.standartKeyDate})";
            (baseRect.visual[1] as Text).origin = Vector2.Zero;
            (baseRect.visual[1] as Text).pos = new Vector2(62f, 116f);

            (baseRect.visual[0] as Sprite).scale = DetermentSize() - outlineSize;
            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.85f);
        }
    }
}
