using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class StatusManager
    {
        Maincode mc;

        public StatusShow show;

        public void SetNoFade(bool value)
        {
            show.isNoFade = value;
        }

        public void SetLog(string text)
        {
            show.SetText(text);
        }

        public void Update(float deltaTime)
        {

        }

        public StatusManager(Maincode imc)
        {
            mc = imc;
        }
    }
}
