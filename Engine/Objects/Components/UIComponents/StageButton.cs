using Engine.Objects.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    class StageButton : Button
    {
        public override void Click(TestGame game)
        {
            MenuLoader.ClearMenu(game);
            Stages.StageManager.LoadStage(data.i1, game);
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
    }
}
