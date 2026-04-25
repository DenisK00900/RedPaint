using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RedPaint
{
    public class DialogPromnt : DialogBox
    {
        InputBox promntBox;
        InputBox nPromntBox;

        public override DialogPromnt Clone()
        {
            DialogPromnt clone = new DialogPromnt(mc, parent);

            return clone;
        }

        public override void SetDepth(int depth)
        {
            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);
            setRect.SetDepth(depth + 2);

            if (isCreated)
            {
                promntBox.SetDepth(depth + 4);
                nPromntBox.SetDepth(depth + 4);
            }
        }

        public override Vector2 DetermentSize()
        {
            return new Vector2(700f, 200f);
        }

        public void OnFinishWrite()
        {
            mc._data.promnt = promntBox.stringInput;
            mc._data.negativePromnt = nPromntBox.stringInput;
        }

        public override void Update(float deltaTime)
        {
            SetDepth(1000);

            base.Update(deltaTime);
        }

        public override void OnDrop()
        {
            promntBox.UpdateHitbox();
            nPromntBox.UpdateHitbox();

            base.OnDrop();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            mc._entityManager.AddEntity(promntBox);
            mc._entityManager.AddEntity(nPromntBox);

            promntBox.UpdateHitbox();
            nPromntBox.UpdateHitbox();
        }

        public DialogPromnt(Maincode imc, AbstrEntity pr = null) : base(imc, pr)
        {
            setRect.headText = "Запрос";

            promntBox = new InputBox(mc, baseRect);
            nPromntBox = new InputBox(mc, baseRect);

            promntBox.includeAlp = true;
            promntBox.stringInput = mc._data.promnt;
            promntBox.SetSize(580f);
            promntBox.SetPos(310f, 84f);

            nPromntBox.includeAlp = true;
            nPromntBox.stringInput = mc._data.negativePromnt;
            nPromntBox.SetSize(580f);
            nPromntBox.SetPos(310f, 164f);

            promntBox.onFinishWrite += OnFinishWrite;
            nPromntBox.onFinishWrite += OnFinishWrite;

            baseRect.visual = new VisualElement[3];

            baseRect.visual[0] = new Sprite(baseRect);
            (baseRect.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Misc/plane");

            baseRect.visual[1] = new Text(baseRect);
            (baseRect.visual[1] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[1] as Text).text = "Запрос";
            (baseRect.visual[1] as Text).origin = Vector2.Zero;
            (baseRect.visual[1] as Text).pos = new Vector2(20f, 40f);

            baseRect.visual[2] = new Text(baseRect);
            (baseRect.visual[2] as Text).font = mc.Content.Load<SpriteFont>("Fonts/Haipapikuseru/Haipapikuseru1");
            (baseRect.visual[2] as Text).text = "Негативный запрос";
            (baseRect.visual[2] as Text).origin = Vector2.Zero;
            (baseRect.visual[2] as Text).pos = new Vector2(20f, 120f); 

            (baseRect.visual[0] as Sprite).scale = DetermentSize() - outlineSize;
            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
            Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.85f);
        }
    }
}
