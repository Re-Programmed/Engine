using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Components
{
    interface Component
    {
        public enum ComponentType
        { 
            SpriteAnimator,
            BoxCollider,
            Script,
            PhysicsRel,
            Trigger_Layer,
            UI_Button_Stage,
            UI_Button_Close
        }

        public abstract ComponentType GetComponentType();

        public abstract void Init(GameObject parent);
        public abstract void Update(TestGame game);

        public abstract void OnReEnable(TestGame game);
        public abstract void OnDisable(TestGame game);

        public abstract T GetClone<T>();

    }
}
