using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Physics.Collision
{
    class BoxCollider : Collider
    {
        internal Vector2 scale = new Vector2(1f, 1f);
        internal Vector2 center = new Vector2(0, 0);
        internal int layer = 4;

        /// <summary>
        /// Create a box collider with a scale and center.
        /// </summary>
        public BoxCollider(Vector2 scale, Vector2 center, int layer)
        {
            this.scale = scale;
            this.center = center;
            this.layer = layer;
            PhysicsManager.RegisterCollider(this);
        }

        public void Disable()
        {
            PhysicsManager.RemoveCollider(this);
        }

        public void Enable()
        {
            PhysicsManager.RegisterCollider(this);
        }

        public void UpdateCenter(Vector2 center)
        {
            this.center = center;
        }

        public void UpdateLayer(int layer)
        {
            this.layer = layer;
        }

        public Vector2 getActiveCollisions()
        {
            foreach (Collider colCheck in PhysicsManager.colliders)
            {
                if (colCheck == this) { continue; }
                
                Vector2 d = CollisionDetection.DetectCollision(this, colCheck);
                if (d.Y != 0 || d.X != 0)
                {
                    return d;
                }
            }

            return Vector2.Zero;
        }
    }
}
