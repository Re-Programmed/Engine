﻿using Engine.Objects.Components.UIComponents;
using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Objects.Components.Component;

namespace Engine.Objects.Components
{
    class ComponentData
    {
        public float radius { get; set; } = 1f;
        public Component.ComponentType type { get; set; }

        public int i1 { get; set; } = 0;

        public Trigger GenerateTriggerFromData()
        {
            if (type == ComponentType.Trigger_Layer)
            {
                LayerSwitch ls = new LayerSwitch(this);
                return ls;
            }

            return null;
        }

        public Button GenerateButtonFromData()
        {
            if(type == ComponentType.UI_Button_Stage)
            {
                StageButton bs = new StageButton(this);
                return bs;
            }

            return null;
        }
    }
}