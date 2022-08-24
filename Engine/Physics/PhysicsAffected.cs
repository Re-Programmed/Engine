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
            if (GameTime.TimeScale == 0) { return; }
            collider.UpdateCenter(gameObject.position);
            
            if(!settings.Static)
            {
                if (settings.Gravity)
                {
                    velocity.AddVelocity(new Vector2(settings.GravityDirectionX, settings.GravityDirectionY) * settings.GravityStrength * 0.001f * GameTime.TimeScale);
                }

                bool airres = false;

                if(velocity.GetVelocity().X < -0.001f)
                {
                    velocity.AddVelocity(Utils.Math.RightVector * settings.AirResistance * GameTime.TimeScale);
                    airres = true;
                }

                if (velocity.GetVelocity().X > 0.001f)
                {
                    velocity.AddVelocity(Utils.Math.LeftVector * settings.AirResistance * GameTime.TimeScale);
                    airres = true;
                }

                if(!airres)
                {
                    velocity.SetVelocity(0f, velocity.GetVelocity().Y);
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

                    if(col.X < 0)
                    {
                        if(velocity.GetVelocity().X > 0)
                        {
                            velocity.SetVelocity(new Vector2(0, velocity.GetVelocity().Y));
                        }
                    }

                    if(col.X > 0)
                    {
                        if(velocity.GetVelocity().X < 0)
                        {
                            velocity.SetVelocity(new Vector2(0, velocity.GetVelocity().Y));
                        }
                    }
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

        protected ComponentData myData;

        public Component SetComponentData(ComponentData data)
        {
            myData = data;
            data.type = GetComponentType();
            return this;
        }

        public ComponentData GetComponentData()
        {
            if (myData == null) { return new ComponentData(); }
            return myData;
        }
    }
}
