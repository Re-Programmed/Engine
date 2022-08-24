using Engine.Input.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    static class ButtonSelectionManager
    {
        const GLFW.Keys KeyAdvance = GLFW.Keys.Right;
        const GLFW.Keys KeyBackTrack = GLFW.Keys.Left;

        static int currentSelection = -1;

        internal static bool UsingSelection { get; private set; }

        /// <summary>
        /// Move the current selected button forward.
        /// </summary>
        /// <param name="amount">How much to advance by. (1: move forward, -1 move to previous.)</param>
        public static void AdvanceSelection(int amount)
        {
            UsingSelection = true;
            DeselectCurrent();

            currentSelection += amount;

            if (currentSelection >= Button.buttons_ordered.Count)
            {
                CancelSelection();
            }

            SelectCurrent();
        }

        /// <summary>
        /// Use the mouse to select.
        /// </summary>
        static void CancelSelection()
        {
            UsingSelection = false;
            DeselectAll();
            currentSelection = -1;
        }

        static void DeselectAll()
        {
            foreach(Button b in Button.buttons_ordered)
            {
                b.Deselect();
            }
        }

        static void SelectCurrent()
        {
            try
            {
                if (currentSelection != -1)
                {
                    Button.buttons_ordered[currentSelection].Select();
                }
            }
            catch(Exception e)
            {
                //Button is gone
                currentSelection = -1;
            }
        }

        static void DeselectCurrent()
        {
            try
            {
                if (currentSelection != -1)
                {
                    Button.buttons_ordered[currentSelection].Deselect();
                }
            }catch(Exception e)
            {
                //Button is gone
                currentSelection = -1;
            }
        }

        static Vector2 lastMousePosition;

        static bool advanceDown, backTrackDown;

        /// <summary>
        /// Called every frame to check for selection of a button.
        /// </summary>
        /// <param name="game">The current game running.</param>
        public static void RecieveInput(TestGame game)
        {
            if(lastMousePosition != Input.Input.GetMousePosition())
            {
                lastMousePosition = Input.Input.GetMousePosition();

                CancelSelection();
            }

            if(Input.Input.GetKey(KeyAdvance))
            {
                if(!advanceDown)
                {
                    AdvanceSelection(1);
                    advanceDown = true;
                }
            }
            else
            {
                advanceDown = false;
            }

            if (Input.Input.GetKey(KeyBackTrack))
            {
                if (!backTrackDown)
                {
                    AdvanceSelection(-1);
                    backTrackDown = true;
                }
            }
            else
            {
                backTrackDown = false;
            }

            if(KeybindManager.GetKeybind("button_s"))
            {
                if(currentSelection > -1 && currentSelection < Button.buttons_ordered.Count)
                {
                    Button.buttons_ordered[currentSelection].Click(game);
                }
            }
        }
    }
}
