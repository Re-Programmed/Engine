using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Physics
{
    class PhysicsManager
    {
        public static List<Collider> colliders { get; private set; } = new List<Collider>();

        public static void RegisterCollider(Collider col)
        {
            if(!colliders.Contains(col))
            {
                colliders.Add(col);
            }
        }

        public static void RemoveCollider(Collider col)
        {
            if(colliders.Contains(col))
            {
                colliders.Remove(col);
            }
        }

        public static PhysicsObjectSettings GeneralPhysicsSettings()
        {
            return new PhysicsObjectSettings(true, 9.81f / 50f, Engine.Utils.Math.DownVector, false, 0.002f);
        }
    }
}
