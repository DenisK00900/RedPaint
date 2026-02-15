using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public class SettingsManager
    {
        Maincode parent;

        public SettingsManager(Maincode mc)
        {
            parent = mc;
        }

        public void SetResolution()
        {
            parent._graphics.PreferredBackBufferWidth = (int)parent._data.res.X;
            parent._graphics.PreferredBackBufferHeight = (int)parent._data.res.Y;
        }

        public void SetFullScreen()
        {
            parent._graphics.IsFullScreen = parent._data.isFullScreen;
        }

        public void ApplyChanges()
        {
            parent._graphics.ApplyChanges();
        }

        public AppPalletre GetCurrPalletre()
        {
            return parent._data.LoadedPalletres[parent._data.currPalletre];
        }
    }
}