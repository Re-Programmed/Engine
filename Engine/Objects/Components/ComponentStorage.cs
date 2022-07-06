using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Objects.Components.Component;

namespace Engine.Objects.Components
{
    class ComponentStorage
    {
        static readonly Component[] components = { new BoxCollider(), new SpriteAnimator(), new Physics.PhysicsAffected() };

        public static Component GetComponentByType(ComponentType type)
        {
            foreach(Component c in components)
            {
                if(c.GetComponentType() == type)
                {
                    return c.GetClone<Component>();
                }
            }

            return null;
        }
    }
}
