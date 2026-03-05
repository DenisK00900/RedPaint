using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPaint
{
    public interface IPrerender
    {
        public void Prerender(SpriteBatch sb);
    }
}
