using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Components
{
    class LayerSwitch : Trigger
    {
        public override Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.Trigger_Layer;
        }

        public override void OnTrigger()
        {
            track.SetLayer(data.i1);
        }

        public void SetLayer(int layer)
        {
            data.i1 = layer;
        }

        public LayerSwitch()
        {
            data.type = GetComponentType();
            track = TestGame.GetDefaultTriggerObject();
        }

        public LayerSwitch(ComponentData td)
            :base(td)
        {
            
        }

        public LayerSwitch(GameObject track)
            : base(track)
        {
            data.type = GetComponentType();
        }
    }
}
