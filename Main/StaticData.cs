using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class StaticData
    {
        Maincode parent;

        public StaticData(Maincode mc)
        {
            parent = mc;

            res = new Vector2(1920, 1080);

            isFullScreen = false;

            currPalletre = 0;

            LoadedPalletres = new AppPalletre[1];
            LoadedPalletres[0] = new AppPalletre();

            version = "Build 7";
        }

        public Vector2 res;

        public bool isFullScreen;

        public int currPalletre;

        public AppPalletre[] LoadedPalletres;

        public String version;
    }
}
