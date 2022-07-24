using Engine.Objects.UI;
using Engine.Rendering.Display;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    /// <summary>
    /// A button used to close the game.
    /// </summary>
    class QuitButton : Button
    {
        Vector3 SelectColor = Vector3.UnitX;

        public override void Click(TestGame game)
        {
            GLFW.Glfw.SetWindowShouldClose(DisplayManager.Window, true);
        }

        public override void Hover()
        {
            gameObject.color = SelectColor;
        }

        public override void ReleaseHover()
        {
            gameObject.color = Vector3.One;
        }

        public override Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.UI_Button_Close;
        }

        public QuitButton()
        {
            this.data.type = Component.ComponentType.UI_Button_Close;
        }

        public QuitButton(ComponentData td)
        {
            this.data = td;
            this.data.type = Component.ComponentType.UI_Button_Close;
        }

        public override void Select()
        {
            base.Select();
            gameObject.color = SelectColor;
        }

        public override void Deselect()
        {
            base.Deselect();
            gameObject.color = Vector3.One;
        }
    }
}
