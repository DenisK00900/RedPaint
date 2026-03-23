using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class GetColor : AbstrTool
    {
        public BlockRender blockRender;

        public GetColor(Maincode imc) : base(imc)
        {
            name = "Пипетка";
            icon = mc.Content.Load<Texture2D>("Texture/Icons/Tools/IconGetColor");
            dest = "Устанавливает цвет пикселя\nв качестве цвета рисования";

            blockRender = new BlockRender(mc);
        }

        public override Texture2D GetPrerender(float scale = 1f)
        {
            blockRender.size = (int)(scale);

            blockRender.thickness = 2f;

            blockRender.Generate();

            return blockRender.Tex;
        }

        public override List<ToolSet> GetSets()
        {
            follows.Clear();
            return new List<ToolSet>();
        }

        public override void Update(float deltaTime)
        {
            if (!mc._image.CanWrite()) return;

            if (mc._input.IsPressed(Button.LeftButton))
            {
                var texture = mc._image.GetCurrentImage();
                var texPos = GetTexPos();

                if (texture != null && IsInTextureBounds(texPos))
                {
                    int x = (int)Math.Round(texPos.X);
                    int y = (int)Math.Round(texPos.Y);

                    Color pickedColor = mc._image.GetPixel(x, y);
                    mc._image.SetColor(pickedColor, pickedColor.A * 255);
                }
            }
        }

        private bool IsInTextureBounds(Vector2 pos)
        {
            var tex = mc._image.GetCurrentImage();
            return tex != null &&
                   pos.X >= 0 && pos.X < tex.Width &&
                   pos.Y >= 0 && pos.Y < tex.Height;
        }
    }
}