using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class ActionLoadFile : Action
    {
        public override void Act()
        {
            //При вызове откроет диалоговое окно виндвос и выбор файла типа .png / .jpg
        }
        public ActionLoadFile(Maincode imc) : base(imc)
        {

        }
    }
}
