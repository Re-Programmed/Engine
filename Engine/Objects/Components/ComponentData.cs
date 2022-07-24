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

        /// <summary>
        /// Returns a class corrasponding to the ComponentType enum.
        /// </summary>
        public Button GenerateButtonFromData()
        {
            switch(type)
            {
                case ComponentType.UI_Button_Close:
                    return new QuitButton(this);
                case ComponentType.UI_Button_Stage:
                    return new StageButton(this);
                default:
                    return null;
            }
        }
    }
}
