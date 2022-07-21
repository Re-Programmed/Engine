using Engine.Game;
using Engine.Input.Utils;
using Engine.Objects;
using Engine.Objects.Stages;
using Engine.Objects.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.GameFiles.Menus
{
    static class PauseMenuManager
    {
        const string PauseBind = "pause";

        static bool menuOpen = false;
        static bool keyDown = false;

        static GameObject menu;

        public static void OpenPauseMenu(TestGame game)
        {
            menuOpen = true;

            menu = GameObject.CreateGameObjectSprite(Vector2.Zero, Vector2.One, 0f, game.sr.verts, "none");

            game.Instantiate(menu, 4);

            MenuLoader.LoadMenu("pause", game, menu);

            GameTime.TimeScale = 0f;
        }

        public static void ClosePauseMenu(TestGame game)
        {
            menuOpen = false;
            if(menu != null)
            {
                menu.DestroyWithChildren(game);
            }

            GameTime.TimeScale = 1f;
        }

        public static void UpdateCheckKey(TestGame game)
        {
            if(KeybindManager.GetKeybind(PauseBind))
            {
                if(!keyDown)
                {
                    keyDown = true;
                    if (menuOpen)
                    {
                        ClosePauseMenu(game);
                    }
                    else
                    {
                        OpenPauseMenu(game);
                    }
                }
            }
            else
            {
                keyDown = false;
            }
        }
    }
}
