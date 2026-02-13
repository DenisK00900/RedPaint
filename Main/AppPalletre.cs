using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RedPaint
{
    public class AppPalletre
    {
        public AppPalletre(
            Color bc1, 
            Color bc2,
            Color ic1,
            Color ic2,
            Color bc,
            Color tc1,
            Color tc2,
            Color dtc,
            Color ec1,
            Color ec2
            )
        {
            baseColor1 = bc1;
            baseColor2 = bc2;
            itemColor1 = ic1;
            itemColor2 = ic2;
            boxColor = bc;
            textColor1 = tc1;
            textColor2 = tc2;
            effectColor1 = ec1;
            effectColor2 = ec2;
        }

        public AppPalletre()
        {
            baseColor1 = new Color(85, 85, 85);
            baseColor2 = new Color(55, 55, 55);

            itemColor1 = new Color(60, 70, 190);
            itemColor2 = new Color(20, 20, 80);

            boxColor = new Color(240, 240, 240);

            textColor1 = new Color(240, 240, 240);
            textColor2 = new Color(10, 10, 10);

            effectColor1 = new Color(40, 240, 30);
            effectColor2 = new Color(65, 220, 235);
        }

        public Color baseColor1;
        public Color baseColor2;

        public Color itemColor1;
        public Color itemColor2;

        public Color boxColor;

        public Color textColor1;
        public Color textColor2;

        public Color effectColor1;
        public Color effectColor2;
    }
}
