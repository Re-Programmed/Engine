using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine.Rendering.Display;
using GLFW;
using static Engine.OpenGL.GL;

#pragma warning disable

namespace Engine.Input
{
    static class Input
    {
        public static bool GetKey(Keys key)
        {
            return (Glfw.GetKey(DisplayManager.Window, key) == InputState.Press);
        }

        public static Vector2 GetMousePosition()
        {
            double x = -1, y = -1;
            Glfw.GetCursorPosition(DisplayManager.Window, out x, out y);

            return new Vector2((float)x, (float)y);
        }

        public static bool GetMouseButton(MouseButton mb)
        {
            return (Glfw.GetMouseButton(DisplayManager.Window, mb) == InputState.Press);
        }
        
    }
}
