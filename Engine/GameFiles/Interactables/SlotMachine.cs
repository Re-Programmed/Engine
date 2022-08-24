using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.GameFiles.Interactables
{
    class SlotMachine : Interactable
    {
        public override T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        public override Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.Interactable_Slots;
        }

        public override void Interact()
        {
            gameObject.color = new System.Numerics.Vector3(1, 0, 0);
        }
    }
}
