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
            baseTools.AddTool(new Pencil(tb.mc));
            baseTools.AddTool(new Fill(tb.mc));
            baseTools.AddTool(new Erase(tb.mc));
            tb.AddRegion(baseTools);

            ToolsRegion SelectTools = new ToolsRegion(tb.mc, tb);
            SelectTools.SetHeadText("Выделение");
            SelectTools.AddTool(new RectSelect(tb.mc));
            tb.AddRegion(SelectTools);

            ToolsRegion otherTools = new ToolsRegion(tb.mc, tb);
            otherTools.SetHeadText("Другое");
            otherTools.AddTool(new See(tb.mc));
            tb.AddRegion(otherTools);
        }
    }
}
