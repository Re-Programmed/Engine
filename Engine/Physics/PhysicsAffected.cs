using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine;
using Engine.Game;
using Engine.Objects;
using Engine.Objects.Components;

namespace Engine.Physics
{
    class PhysicsAffected : Component
    {
        public PhysicsObjectSettings settings = PhysicsManager.GeneralPhysicsSettings();

        GameObject gameObject;

        public VelocityVector velocity = new VelocityVector(Vector2.One * 1f);

        /// <summary>
        /// True if colliding on the top of an object. Useful for jumping.
        /// </summary>
        public bool collisionOnTop { get; private set; } = false;

        Collider collider;
        
        public Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.PhysicsRel;
        }

        public void Init(GameObject parent)
        {
            collider = new Collision.BoxCollider(parent.scale, parent.position, parent.Layer);
            settings.SetGravity(true, settings.GravityStrength);
            gameObject = parent;
        }

        public PhysicsAffected SetStatic(bool stat)
        {
            settings.SetStatic(stat);
            return this;
        }

        public void UpdateLayer(int layer)
        {
            collider.UpdateLayer(layer);
        }

        public void Update(TestGame game)
        {
            collider.UpdateCenter(gameObject.position);
            
            if(!settings.Static)
            {
                if (settings.Gravity)
                {
                    velocity.AddVelocity(new Vector2(settings.GravityDirectionX, settings.GravityDirectionY) * settings.GravityStrength * 0.001f);
                }

                if(velocity.GetVelocity().X < 0)
                {
                    velocity.AddVelocity(Utils.Math.RightVector * settings.AirResistance);
                }

                if (velocity.GetVelocity().X > 0)
                {
                    velocity.AddVelocity(Utils.Math.LeftVector * settings.AirResistance);
                }

                Vector2 col = collider.getActiveCollisions();
                collisionOnTop = false;
                if(col.Y != 0)
                {
                    if(col.Y < 0)
                    {
                        collisionOnTop = true;
                    }
                    gameObject.Translate(col);
                    velocity.SetVelocity(new Vector2(velocity.GetVelocity().X, 0));
                }

                if (col.X != 0)
                {
                    gameObject.Translate(col);
                    velocity.SetVelocity(new Vector2(0, velocity.GetVelocity().Y));
                }

                velocity.SendFrameVelocityData(gameObject);
            }
        }
        public void OnReEnable(TestGame game)
        {
            collider.Enable();
        }

        public void OnDisable(TestGame game)
        {
            collider.Disable();
        }

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }
    }
}
