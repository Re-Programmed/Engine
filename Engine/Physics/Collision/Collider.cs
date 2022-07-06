using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Engine.Physics
{
    interface Collider
    {
        public Vector2 getActiveCollisions();
        public void Disable();
        public void Enable();

        public void UpdateCenter(Vector2 center);
        public void UpdateLayer(int layer);
    }
}
