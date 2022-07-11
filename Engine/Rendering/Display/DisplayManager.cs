using GLFW;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;
using static Engine.OpenGL.GL;

namespace Engine.Rendering.Display
{
    static class DisplayManager
    {
        public static Window Window { get; set; }
        public static Vector2 WindowSize { get; set; }

        public static void SetBordered(bool bordered)
        {
            Glfw.SetWindowAttribute(Window, WindowAttribute.Decorated, bordered);
        }

        public static void SetResizable(bool resizeable)
        {
            Glfw.SetWindowAttribute(Window, WindowAttribute.Resizable, resizeable);
        }

        public static void SetAspectRatio(byte n, byte d)
        {
            Glfw.SetWindowAspectRatio(Window, n, d);
        }

        public static void Maximize()
        {
            Glfw.MaximizeWindow(Window);
        }

        public static void SetTitle(string title)
        {
            Glfw.SetWindowTitle(Window, title);
        }

        public static void Fullscreen()
        {
            WindowSize = new Vector2(Glfw.PrimaryMonitor.WorkArea.Width, Glfw.PrimaryMonitor.WorkArea.Height);
            Glfw.SetWindowMonitor(Window, Glfw.PrimaryMonitor, 0, 0, (int)WindowSize.X, (int)WindowSize.Y, Glfw.GetVideoMode(Glfw.PrimaryMonitor).RefreshRate);
        }

        public static void CreateWindow(int width, int height, string title)
        {
            WindowSize = new Vector2(width, height);

            Glfw.Init();

            //Use OpenGL 3.3
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);

            //Make the window focused and not resizeable
            Glfw.WindowHint(Hint.Focused, true);
            
            Glfw.WindowHint(Hint.Resizable, false);
                
            //Create Window
            Window = Glfw.CreateWindow(width, height, title, Monitor.None, Window.None);

            if (Window == Window.None)
            {
                //Error
                return;
            }

            Rectangle screen = Glfw.PrimaryMonitor.WorkArea;

            int x = (screen.Width - width) / 2;
            int y = (screen.Height - height) / 2;

            Glfw.SetWindowPosition(Window, x, y);

            Glfw.MakeContextCurrent(Window);
            Import(Glfw.GetProcAddress);

            glViewport(0, 0, width, height);
            Glfw.SwapInterval(0); //Vsync off, 1 is on

            Fullscreen();
        }

        public static void CloseWindow()
        {
            Glfw.Terminate();
        }
    }
}
