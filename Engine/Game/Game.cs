using Engine.Objects;
using Engine.Rendering.Cameras;
using Engine.Rendering.Display;
using GLFW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Game
{
    abstract class Game
    {

        public static float AnimationSync;

        public static int AnimationFrames;

        public Camera2d cam;

        protected int InitWindowWidth { get; set; }
        protected int InitWindowHeight { get; set; }
        protected string InitWindowTitle { get; set; }

        public Dictionary<int, ObjectLayer> objects = new Dictionary<int, ObjectLayer>();

        public Dictionary<int, ObjectLayer> UI = new Dictionary<int, ObjectLayer>();

        public Game(int initWindowWidth, int initWindowHeight, string initWindowTitle)
        {
            InitWindowWidth = initWindowWidth;
            InitWindowHeight = initWindowHeight;
            InitWindowTitle = initWindowTitle;
        }

        public void Run()
        {
            DisplayManager.CreateWindow(InitWindowWidth, InitWindowHeight, InitWindowTitle);

            LoadContent();
            Initalize();

            while (!Glfw.WindowShouldClose(DisplayManager.Window))
            {
                GameTime.DeltaTime = (float)Glfw.Time - GameTime.TotalElapsedSeconds;
                GameTime.TotalElapsedSeconds = (float)Glfw.Time;

                AnimationSync += GameTime.DeltaTime;

                if (AnimationSync >= 0.1f)
                {
                    AnimationFrames++;
                    if(AnimationFrames == 4)
                    {
                        AnimationFrames = 0;
                    }

                    AnimationSync = 0;
                }

                Update();

                Glfw.PollEvents();

                Render();

            }

            Close();

            DisplayManager.CloseWindow();
        }

        protected abstract void Initalize();
        protected abstract void LoadContent();

        protected abstract void Update();
        protected abstract void Render();

        protected abstract void Close();
    }
}
