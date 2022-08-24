using Engine.Objects.Components;
using Engine;
using Engine.Input;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine.Utils;
using Engine.Game;
using Engine.DevUtils;
using Engine.Input.Utils;

namespace Engine.GameFiles
{
    class Player : ScriptComponent
    {
        Physics.PhysicsAffected comp;
        public override void Start()
        {
            comp = gameObject.GetComponent<Physics.PhysicsAffected>(Component.ComponentType.PhysicsRel);
        }

        public override void ScriptUpdate(TestGame game)
        {
            game.cam.LerpTwards(gameObject.position, 0.01f);
            if(gameObject.position.Y > 500)
            {
                gameObject.SetPosition(new Vector2(gameObject.position.X, -500));
            }
            if (Input.Input.GetKey(GLFW.Keys.L))
            {
                if(TestGame.INSTANCE.DEVELOPER_MODE)
                {
                    gameObject.AddComponent(new DebugTool());
                    gameObject.RemoveComponent(comp);
                    gameObject.RemoveComponent(this);
                }
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

        const float Speed = 0.0025f;
        const float VelocityCap = 0.5f;

        public void CalcInputs()
        {
            if (KeybindManager.GetKeybind("forward"))
            {
                gameObject.texture.flipped = false;
                comp.velocity.AddVelocity(Utils.Math.RightVector * Speed * GameTime.TimeScale);

                if (comp.velocity.GetVelocity().X > VelocityCap)
                {
                    comp.velocity.SetVelocity(VelocityCap, comp.velocity.GetVelocity().Y);
                }
             
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

            if (KeybindManager.GetKeybind("backward"))
            {
                gameObject.texture.flipped = true;
                comp.velocity.AddVelocity(Utils.Math.LeftVector * Speed * GameTime.TimeScale);

                if (comp.velocity.GetVelocity().X < -VelocityCap)
                {
                    comp.velocity.SetVelocity(-VelocityCap, comp.velocity.GetVelocity().Y);
                }

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

            if (KeybindManager.GetKeybind("jump") && comp.collisionOnTop)
            {
                comp.velocity.AddVelocity(new Vector2(0, -0.3f));
            }
        }
    }
}
