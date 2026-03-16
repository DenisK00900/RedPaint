using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionNewPanel : AbstrAction
    {
        public override void Act()
        {
            if (mc.mainHolder.map.Count > 0)
            {
                mc.mainHolder.AddPanel(new Panel(mc), mc.mainHolder.map[0]);
            }
            else
            {
                mc._entityManager.AddEntity(
                    new DialogError(
                        mc,
                        "Ошибка панелей",
                        "Нет доступного места. Освободите место, прежде чем создать новую панель",
                        null
                        ));
            }
        }
        public ActionNewPanel(Maincode imc) : base(imc)
        {

        }
    }
}
