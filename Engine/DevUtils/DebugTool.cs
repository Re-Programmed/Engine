using Engine.GameFiles;
using Engine.Objects.Components;
using Engine.Objects.Stages;
using Engine;
using Engine.Game;
using Engine.Input;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine.Objects.UI;

namespace Engine.DevUtils
{
    class DebugTool : ScriptComponent
    {
        GameObject currentObjectToPlace;

        static List<GameObject> debugObjects;
        int curobj = 0;

        float gridSize = 0.1f;

        public override void Start()
        {
           
        }

        private void GenerateDebugObjects(TestGame game)
        {
            debugObjects = new List<GameObject>();

            GameObject testGround = GameObject.CreateGameObjectSprite(Vector2.Zero, new Vector2(50f, 50f), 0f, game.sr.verts, "black");
            testGround.AddComponent(new Physics.PhysicsAffected().SetStatic(true));
            debugObjects.Add(testGround);

            GameObject trigger = GameObject.CreateGameObjectSprite(Vector2.Zero, Vector2.One * 25f, 0f, game.sr.verts, "test_sp");

            LayerSwitch ls = new LayerSwitch();
            ls.SetRadius(25f);
            ls.SetLayer(1);

            trigger.AddComponent(ls);

            debugObjects.Add(trigger);

            currentObjectToPlace = debugObjects[0];
        }

        public void InheritObjData(GameObject data, TestGame game)
        {
            gameObject.SetScale(data.scale);
            gameObject.SetRotation(data.rotation);

            gameObject.texture.UpdateTexture(data.texture.textureName);
            gameObject.texture.flipped = data.texture.flipped;

            currentObjectToPlace = data;
        }

        bool mouseDown = false;
        bool mouseDownR = false;
        bool XDown = false;

        bool changeStage = false;

        bool SaveDown = false;

        bool lockToAxis, lockDown, lockedMotionDown = false;


        public override void ScriptUpdate(TestGame game)
        {
            game.cam.LerpTwards(gameObject.position, 0.01f);
            if(Input.Input.GetKey(GLFW.Keys.G))
            {
                if(!lockDown)
                {
                    if (!lockToAxis)
                    {
                        gridSize *= 10f;
                        if(gridSize > 9999f)
                        {
                            gridSize = 0.1f;
                        }
                    }
                    lockDown = true;
                    gameObject.SetPosition(new Vector2((float)Math.Round(gameObject.position.X / gridSize)  * gridSize, (float)Math.Round(gameObject.position.Y / gridSize) * gridSize));
                    lockToAxis = !lockToAxis;
                }
            }
            else { lockDown = false; }

            if (debugObjects == null)
            {
                GenerateDebugObjects(game);
            }

            if(Input.Input.GetKey(GLFW.Keys.K))
            {
                gameObject.AddComponent(new Player());
                gameObject.AddComponent(new Physics.PhysicsAffected());
                gameObject.texture.UpdateTexture("test_sp");
                gameObject.SetScale(new Vector2(40f, 56f));
                gameObject.RemoveComponent(this);
            }

            if(Input.Input.GetKey(GLFW.Keys.Right))
            {
                if(!changeStage)
                {
                    changeStage = true;
                    LoadNextStage(game);
                }
            }
            else
            {
                changeStage = false;
            }

            bool resetLock = true;

            if(Input.Input.GetKey(GLFW.Keys.D))
            {
                resetLock = false;
                if(lockToAxis)
                {
                    if (!lockedMotionDown)
                    {
                        gameObject.Translate(Utils.Math.RightVector * gridSize);
                        lockedMotionDown = true;
                    }
                }
                else
                {
                    gameObject.Translate(Utils.Math.RightVector * GameTime.DeltaTime * 300f);
                }
            }
            

            if (Input.Input.GetKey(GLFW.Keys.A))
            {
                resetLock = false;
                if (lockToAxis)
                {
                    if (!lockedMotionDown)
                    {
                        gameObject.Translate(Utils.Math.LeftVector * gridSize);
                        lockedMotionDown = true;
                    }
                }
                else
                {
                    gameObject.Translate(Utils.Math.LeftVector * GameTime.DeltaTime * 300f);
                }
            }
            

            if (Input.Input.GetKey(GLFW.Keys.W))
            {
                resetLock = false;
                if (lockToAxis)
                {
                    if (!lockedMotionDown)
                    {
                        gameObject.Translate(Utils.Math.UpVector * gridSize);
                        lockedMotionDown = true;
                    }
                }
                else
                {
                    gameObject.Translate(Utils.Math.UpVector * GameTime.DeltaTime * 300f);
                }
            }
            

            if (Input.Input.GetKey(GLFW.Keys.S))
            {
                resetLock = false;
                if (lockToAxis)
                {
                    if (!lockedMotionDown)
                    {
                        gameObject.Translate(Utils.Math.DownVector * gridSize);
                        lockedMotionDown = true;
                    }
                }
                else
                {
                    gameObject.Translate(Utils.Math.DownVector * GameTime.DeltaTime * 300f);
                }
            }

            if(resetLock)
            {
                lockedMotionDown = false;
            }
           

            if (Input.Input.GetKey(GLFW.Keys.X))
            {
                if(!XDown)
                {
                    XDown = true;
                    Console.WriteLine(debugObjects.Count + "<=" + curobj);
                    if (debugObjects.Count <= curobj + 1)
                    {
                        curobj = 0;
                    }
                    else { curobj++; }

                    InheritObjData(debugObjects[curobj], game);
                }
            }
            else
            {
                XDown = false;
            }

            if(Input.Input.GetMouseButton(GLFW.MouseButton.Left))
            {
                if(!mouseDown)
                {
                    mouseDown = true;
                    GameObject newSceneItem = currentObjectToPlace.GetMemberwiseClone();
                    newSceneItem.SetPosition(gameObject.position);

                    game.Instantiate(newSceneItem, 4);

                    /*foreach (Component c in currentObjectToPlace.GetComponents())
                    {
                        if(c.GetComponentType() == Component.ComponentType.PhysicsRel)
                        {
                            Physics.PhysicsAffected physics = (Physics.PhysicsAffected)c;
                            Physics.PhysicsAffected newphy = new Physics.PhysicsAffected();

                            newphy.settings = physics.settings;
                            newphy.SetStatic(physics.settings.Static);

                            newSceneItem.AddComponent(newphy);
                            Console.WriteLine("Added Component TYPE: " + c.GetComponentType());
                        }
                        else
                        {
                            if (c.GetComponentType() != Component.ComponentType.Script)
                            {
                                newSceneItem.AddComponent(c.GetClone<Component>());
                                Console.WriteLine("Added Component TYPE: " + c.GetComponentType());
                            }
                        }
                    }*/
                }
            }
            else
            {
                mouseDown = false;
            }

            if (Input.Input.GetMouseButton(GLFW.MouseButton.Right))
            {
                if(!mouseDownR)
                {
                    mouseDownR = true;
                    foreach (ObjectLayer l in game.objects.Values)
                    {
                        foreach (GameObject o in l.objects)
                        {
                            if (o == gameObject) { continue; }
                            if (Vector2.Distance(o.position, gameObject.position) < 20f)
                            {
                                game.Destroy(o);
                            }
                        }
                    }
                }
            }
            else
            {
                mouseDownR = false;
            }

            if (Input.Input.GetKey(GLFW.Keys.LeftControl))
            {
                if(Input.Input.GetKey(GLFW.Keys.S))
                {
                    if(!SaveDown)
                    {
                        StageManager.UpdateCurrentStage(game);
                        SaveDown = true;
                    }
                }
                else 
                {
                    SaveDown = false;
                }
            }
            else
            {
                SaveDown = false;
            }
        }

        private void LoadNextStage(TestGame game, int increment = 0)
        {
            int i = StageManager.stageId + 1 + increment;
            if (i >= StageManager.StageCount())
            {
                i = 0 + increment;
            }


            if (StageManager.GetStage(i).UI)
            {
                LoadNextStage(game, increment + 1);
            }
            else
            {
                StageManager.LoadStage(i, game);
            }

        }
    }
}
