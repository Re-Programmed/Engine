using Engine.Objects.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    /// <summary>
    /// A button used to load a stage.
    /// </summary>
    class StageButton : Button
    {
        Vector3 SelectColor = Vector3.UnitX;

        public override void Click(TestGame game)
        {
            MenuLoader.ClearMenu(game);
            Stages.StageManager.LoadStage(data.i1, game);
        }

        public override void Hover()
        {

            gameObject.color = SelectColor;
            hovered = true;
            
        }

        public override void ReleaseHover()
        {

            gameObject.color = Vector3.One;
            hovered = false;
            
        }

        public override Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.UI_Button_Stage;
        }

        public StageButton()
        {

        }

        public StageButton(ComponentData td)
            : base(td)
        {

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
