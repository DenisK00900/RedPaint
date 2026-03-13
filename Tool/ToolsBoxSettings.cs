using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace RedPaint
{
    public class ToolsBoxSettings
    {
        public static void SetTools(ToolsBox tb)
        {
            if (!tb.isCreated) throw new InvalidOperationException("ToolsBox должен быть добавлен как объект перед настройкой");

            ToolsRegion baseTools = new ToolsRegion(tb.mc, tb);
            baseTools.SetHeadText("Базовое");
            //for (int i = 0; i < 16; i++)
            baseTools.AddTool(new Pencil(tb.mc));
            tb.AddRegion(baseTools);

            ToolsRegion SelectTools = new ToolsRegion(tb.mc, tb);
            SelectTools.SetHeadText("Выделение");
            tb.AddRegion(SelectTools);

            ToolsRegion otherTools = new ToolsRegion(tb.mc, tb);
            otherTools.SetHeadText("Другое");
            tb.AddRegion(otherTools);
        }
    }
}
