using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Physics
{
    class PhysicsObjectSettings
    {
        public bool Gravity { get; set; }
        public float GravityStrength { get; set; }

        public float AirResistance { get; set; }

        public bool Static { get; set; }

        public float GravityDirectionX { get; set; }
        public float GravityDirectionY { get; set; }

        public PhysicsObjectSettings(bool Gravity, float GravityStrength, Vector2 direction, bool Static, float AirResistance)
        {
            this.Gravity = Gravity;
            this.GravityStrength = GravityStrength;
            GravityDirectionX = direction.X;
            GravityDirectionY = direction.Y;
            this.Static = Static;
            this.AirResistance = AirResistance;
        }

        public PhysicsObjectSettings()
        {

        }

        public void SetStatic(bool isStatic)
        {
            Static = isStatic;
        }

        /// <summary>
        /// Set how gravity will apply to this object.
        /// </summary>
        /// <param name="enabled">Turn gravity on or off.</param>
        /// <param name="value">The strength of gravity.</param>
        public void SetGravity(bool enabled, float value = 9.81f)
        {
            Gravity = enabled;
            GravityStrength = value;
        }

        public void SetGravityDirection(Vector2 direction)
        {
            GravityDirectionX = direction.X;
            GravityDirectionY = direction.Y;
        }
    }
}
