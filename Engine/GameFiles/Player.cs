using Engine.Objects.Components;
using Engine;
using Engine.Input;
using Engine.Objects;
using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine.Utils;
using Engine.Game;
using Engine.DevUtils;

namespace Engine.GameFiles
{
    class Player : ScriptComponent
    {
        Physics.PhysicsAffected comp;
        public override void Start()
        {
            comp = gameObject.GetComponent<Physics.PhysicsAffected>(Component.ComponentType.PhysicsRel);
        }

        bool debug = false;

        public override void ScriptUpdate(TestGame game)
        {
            if (Input.Input.GetKey(GLFW.Keys.L))
            {
                gameObject.AddComponent(new DebugTool());
                gameObject.RemoveComponent(comp);
                gameObject.RemoveComponent(this);
            }
            if (comp == null)
            {
                comp = gameObject.GetComponent<Physics.PhysicsAffected>(Component.ComponentType.PhysicsRel);
            }
            else
            {
                CalcInputs();
            }
        }

        float holdLengthD = 0.004f;
        float holdLengthA = 0.004f;


        public void CalcInputs()
        {
            if (Input.Input.GetKey(GLFW.Keys.D))
            {
                gameObject.texture.flipped = false;
                gameObject.Translate(Utils.Math.RightVector * 400f * GameTime.DeltaTime);
                /*holdLengthD += 0.0000004f;
                if(holdLengthD > 0.01f)
                {
                    holdLengthD = 0.01f;
                }
                comp.velocity.AddVelocity(Utils.Math.RightVector * holdLengthD * 0.3f);
                */
            }
            else
            {
                holdLengthD = 0.004f;
            }

            if (Input.Input.GetKey(GLFW.Keys.A))
            {
                gameObject.texture.flipped = true;
                gameObject.Translate(Utils.Math.LeftVector * 400f * GameTime.DeltaTime);
                /*holdLengthA += 0.0000004f;
                if (holdLengthA > 0.01f)
                {
                    holdLengthA = 0.01f;
                }
                comp.velocity.AddVelocity(Utils.Math.LeftVector * holdLengthA * 0.3f);*/
            }
            else
            {
                holdLengthA = 0.004f;
            }

            if (Input.Input.GetKey(GLFW.Keys.W) && comp.collisionOnTop)
            {
                comp.velocity.AddVelocity(new Vector2(0, -0.3f));
            }
        }
    }
}
