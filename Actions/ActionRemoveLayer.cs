using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionRemoveLayer : AbstrAction
    {
        public int index = 0;

        bool forse = false;

        public override void Act()
        {
            if (!forse)
            {
                if (mc._image.layers.Count == 1)
                {
                    mc._entityManager.AddEntity(
                    new DialogError(
                                mc,
                                "Ошибка удаления слоя",
                                "Нельзя удалить единственный слой",
                                null
                                ));

                    return;
                }

                DialogWarning dw = new DialogWarning(
                                mc,
                                "Ошибка удаления слоя",
                                $"Вы уверены, что хотите удалить слой {index}?",
                                null);

                dw.SetAgreeText("Удалить");

                ActionRemoveLayer forseLoad = new ActionRemoveLayer(mc);
                forseLoad.forse = true;
                forseLoad.index = index;

                dw.agree.AddAction(forseLoad);
                dw.agree.AddAction(new ActionDestroy(mc, dw));

                dw.agree.hint = new Hint(mc, "Удалить этот слой слой");

                dw.SetDisagreeText("Отмена");

                dw.disagree.AddAction(new ActionDestroy(mc, dw));

                dw.disagree.hint = new Hint(mc, "Отменить действие и не удалять слой");

                mc._entityManager.AddEntity(dw);

                return;
            }

            mc._image.RemoveLayer(index);
        }

        public ActionRemoveLayer(Maincode imc) : base(imc)
        {
        }
    }
}
