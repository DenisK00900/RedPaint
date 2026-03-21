using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;

namespace RedPaint
{
    public class LayerBox : AbstrEntity
    {
        public Layer layer;

        public Drawrect baseRect;
        public Drawrect outline;

        public Vector2 outlineSize = new Vector2(8, 8);

        LayerBoxShowNum showNum;

        LayerBoxShowName showName;

        SpriteButton showRemove;

        SpriteButton showEdit;

        CheckBox locker;

        public float leight = 384;

        public int layerIndex = -1;

        public Vector2 targetPos;

        public Vector2 currPos;

        public bool isTaken;

        private int takenIndex;

        public bool canGlow = true;

        public void SetThisLayer()
        {
            mc._image.SetWorkingLayer(layerIndex);
        }

        public override void OnSpawn()
        {
            mc._entityManager.AddEntity(baseRect);
            mc._entityManager.AddEntity(outline);

            mc._entityManager.AddEntity(showNum);

            mc._entityManager.AddEntity(showName);

            mc._entityManager.AddEntity(showEdit);

            mc._entityManager.AddEntity(locker);

            mc._entityManager.AddEntity(showRemove);
        }

        public override void SetDepth(int depth)
        {
            baseRect.SetDepth(depth + 1);
            outline.SetDepth(depth);

            showNum.SetDepth(depth + 2);

            showName.SetDepth(depth + 2);

            locker.SetDepth(depth + 2); 

            showRemove.SetDepth(depth + 2);

            showEdit.SetDepth(depth + 2);

            base.SetDepth(depth);
        }

        public void UpdateHitbox()
        {
            showName.UpdateHitbox();

            showRemove.UpdateHitbox();

            locker.UpdateHitbox();

            showEdit.UpdateHitbox();
        }

        public void SetNum(int num)
        {
            layerIndex = num;

            (showNum.visual[0] as Text).text = num.ToString();

            (showRemove.action[0] as ActionRemoveLayer).index = num;
        }

        public override void Update(float deltaTime)
        {
            if (showName.mouseOver && mc._input.IsPressed(Button.LeftButton))
            {
                isTaken = true;

                showNum.visual[0].color = Color.Yellow;

                takenIndex = layerIndex;
            }
            if (mc._input.IsReleased(Button.LeftButton))
            {
                isTaken = false;

                showNum.visual[0].color = mc._settings.GetCurrPalletre().textColor1;

                if (takenIndex != layerIndex)
                {
                    (parent as LayerSettings).SetLayers();
                }
            }

            currPos = TUH.Lerp(currPos, targetPos, 0.15f);

            SetPos(currPos);

            outline.visual[0].color = 
                (mc._image.workingLayer == layerIndex && canGlow)
                ?
                mc._settings.GetCurrPalletre().effectColor2 :
                Color.Lerp(
                    Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f),
                    mc._settings.GetCurrPalletre().boxColor, 0.15f);

            layer.isLocked = locker.status;

            (showName.visual[0] as Text).text = layer.name;

            base.Update(deltaTime);
        }

        public void DetermentPos(Vector2 pos)
        {
            targetPos = pos;
        }

        public LayerBox(Maincode imc, Layer ilayer, AbstrEntity pr = null) : base(imc, pr)
        {
            layer = ilayer;

            baseRect = new Drawrect(mc, this);
            outline = new Drawrect(mc, baseRect);

            baseRect.position = outlineSize / 2f; 

            outline.SetPos(baseRect.position - outlineSize);

            (baseRect.visual[0] as Sprite).origin = Vector2.Zero;
            (baseRect.visual[0] as Sprite).color =
                Color.Lerp(
                    Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.75f),
                    mc._settings.GetCurrPalletre().boxColor, 0.15f);

            (outline.visual[0] as Sprite).origin = Vector2.Zero;
            (outline.visual[0] as Sprite).color =
                Color.Lerp(
                    Color.Lerp(mc._settings.GetCurrPalletre().baseColor2, mc._settings.GetCurrPalletre().baseColor1, 0.25f),
                    mc._settings.GetCurrPalletre().boxColor, 0.15f);

            baseRect.visual[0].scale = new Vector2(leight, 32f);
            outline.visual[0].scale = new Vector2(leight, 32f) + outlineSize;

            showNum = new LayerBoxShowNum(mc, baseRect); 

            showNum.SetPos(new Vector2(16f, 16f));

            showName = new LayerBoxShowName(mc, baseRect);

            showName.SetPos(new Vector2(16f + 96f, 16f));

            showRemove = new SpriteButton(mc, baseRect);

            showRemove.visual = new VisualElement[1];
            showRemove.visual[0] = new Sprite(showRemove);
            (showRemove.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/cross");
            showRemove.visual[0].scale = new Vector2 (32f / 64f);
            showRemove.AddAction(new ActionRemoveLayer(mc));

            showRemove.SetPos(new Vector2(leight - 16f, 16f));
            showRemove.visual[0].color = mc._settings.GetCurrPalletre().textColor1;

            showRemove.SetHintText("Удалить этот слой и всё его содержимое");

            locker = new CheckBox(mc, baseRect);

            locker.SetPos(new Vector2(leight - 32f - 16f, 16f));
            locker.visual[0].scale = new Vector2(32f / 64f);
            locker.visual[0].color = mc._settings.GetCurrPalletre().textColor1;

            locker.onIcon = mc.Content.Load<Texture2D>("Texture/Icons/lock");
            locker.offIcon = mc.Content.Load<Texture2D>("Texture/Icons/unlock");

            locker.ChangeIcon();

            locker.SetHintText("Заблокировать или разблокировать слой.\nЗаблокированные слои не могут быть изменены");

            showEdit = new SpriteButton(mc, baseRect);

            showEdit.visual = new VisualElement[1];
            showEdit.visual[0] = new Sprite(showEdit);
            (showEdit.visual[0] as Sprite).texture = mc.Content.Load<Texture2D>("Texture/Icons/gear");
            showEdit.visual[0].scale = new Vector2(32f / 64f);

            showEdit.SetPos(new Vector2(leight - 64 - 16f, 16f));
            showEdit.visual[0].color = mc._settings.GetCurrPalletre().textColor1;

            showEdit.SetHintText("Открыть настройки этого слоя");
        }
    }
}
