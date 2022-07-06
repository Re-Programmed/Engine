using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Objects.Components.Component;

namespace Engine.Objects.Components
{
    class TriggerData
    {
        public float radius { get; set; } = 1f;
        public Component.ComponentType type { get; set; }

        public int i1 { get; set; } = 0;

        public Trigger GenerateFromData()
        {
            if (type == ComponentType.Trigger_Layer)
            {
                LayerSwitch ls = new LayerSwitch(this);
                return ls;
            }

            return null;
        }
    }
}
