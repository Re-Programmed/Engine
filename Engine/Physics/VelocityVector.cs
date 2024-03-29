﻿using Engine.Game;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Physics
{
    class VelocityVector
    {
        Vector2 velocity;
        Vector2 velocityCap;

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public VelocityVector(Vector2 cap)
        {
            velocityCap = cap;
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = ClampVelocity(velocity);
        }

        public void AddVelocity(Vector2 velocity)
        {
            this.velocity = ClampVelocity(this.velocity + velocity) * GameTime.TimeScale;
        }

        public void SendFrameVelocityData(GameObject obj)
        {
            obj.Translate(velocity);
        }

        Vector2 ClampVelocity(Vector2 velocity)
        {
            if (velocity.X > velocityCap.X)
            {
                velocity.X = velocityCap.X;
            }
            if (velocity.Y > velocityCap.Y)
            {
                velocity.Y = velocityCap.Y;
            }
            if (velocity.X < -velocityCap.X)
            {
                velocity.X = -velocityCap.X;
            }
            if (velocity.Y < -velocityCap.Y)
            {
                velocity.Y = -velocityCap.Y;
            }
            return velocity;
        }

        internal void SetVelocity(float v1, float v2)
        {
            SetVelocity(new Vector2(v1, v2));
        }
    }
}
