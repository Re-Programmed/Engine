using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Physics.Collision
{
    class CollisionDetection
    {
        /// <summary>
        /// Checks for the collision of collider1 with collider2.
        /// </summary>
        /// <param name="collider1">Collider to treat as base.</param>
        /// <param name="collider2">Secondary.</param>
        /// <returns></returns>
        public static Vector2 DetectCollision(Collider collider1, Collider collider2)
        {
            Vector2 pushOut = new Vector2();
            if(collider1 is BoxCollider && collider2 is BoxCollider)
            {
                BoxCollider b1 = collider1 as BoxCollider;
                BoxCollider b2 = collider2 as BoxCollider;

                if (b1.layer != b2.layer) { return Vector2.Zero; }

                float centerDistanceX = (b1.center.X - b2.center.X);
                float centerDistanceY = (-b1.center.Y) - (-b2.center.Y);
                float scalePushDistanceX = (b1.scale.X / 2f) + (b2.scale.X / 2f);
                float scalePushDistanceY = (b1.scale.Y / 2f) + (b2.scale.Y / 2f);

                if (Math.Abs(centerDistanceY) < scalePushDistanceY && Math.Abs(centerDistanceX) < scalePushDistanceX)
                {
                    float pushCheck = 9999f;

                    if(centerDistanceY < 0)
                    {
                        float cal = Math.Abs(centerDistanceY + scalePushDistanceY);
                        if (pushCheck > cal)
                        {
                            pushCheck = cal;
                            pushOut.Y = (centerDistanceY + scalePushDistanceY) + 0.01f;
                        }
                    }

                    if(centerDistanceY >= 0)
                    {
                        float cal = Math.Abs(scalePushDistanceY - centerDistanceY);
                        if (pushCheck > cal)
                        {
                            pushCheck = cal;
                            pushOut.Y = -(scalePushDistanceY - centerDistanceY) - 0.01f;
                        }
                    }

                    if (centerDistanceX < 0)
                    {
                        float cal = Math.Abs(centerDistanceX + scalePushDistanceX);
                        if (pushCheck > cal)
                        {
                            pushCheck = cal;
                            pushOut.Y = 0f;
                            pushOut.X = -(centerDistanceX + scalePushDistanceX) + 9.01f;
                        }
                    }
                    
                    if (centerDistanceX >= 0)
                    {
                        float cal = Math.Abs(scalePushDistanceX - centerDistanceX);
                        if (pushCheck > cal)
                        {
                            pushOut.Y = 0f;
                            pushOut.X = (scalePushDistanceX - centerDistanceX) + 0.01f;
                        }
                    }
                }
                /*
                if (Math.Abs(centerDistanceX) < scalePushDistanceX)
                {
                    if (centerDistanceX < 0)
                    {
                        pushOut.X = (centerDistanceX + scalePushDistanceX);
                    }

                    if (centerDistanceX >= 0)
                    {
                        pushOut.X = (scalePushDistanceX - centerDistanceX);
                    }
                }
                */
            }

            return pushOut;
        }
    }
}
