﻿using Engine.Game;
using Engine.Objects;
using Engine.Rendering.Cameras;
using Engine.Rendering.Display;
using Engine.Rendering.Shaders;
using GLFW;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Engine.Resources;
using static Engine.OpenGL.GL;
using Engine.Rendering.Sprites;
using Engine.Objects.Components;
using Engine.DevUtils;
using Engine.Rendering;
using Engine.Utils;
using Engine.Resources;
using Engine.SoundSys;
using Engine.Physics;
using Engine.Objects.Stages;
using Engine.DevUtils;
using Engine.GameFiles;

namespace Engine
{
    class TestGame : Game.Game
    {
        public const bool DEVELOPER_MODE = true;

        uint vao;
        uint vbo;

        Shader shader;

        public SpriteRenderer sr { get; private set; }

        protected static Dictionary<IDObject, int> removeObjects = new Dictionary<IDObject, int>();

        protected Camera2d camUI = new Camera2d(Vector2.Zero, 1f);

        static GameObject defaultTriggerObject;
        public static GameObject GetDefaultTriggerObject() { return defaultTriggerObject; }

        public TestGame(int initWindowWidth, int initWindowHeight, string initWindowTitle) : base(initWindowWidth, initWindowHeight, initWindowTitle)
        {

        }

        public static void Destroy(GameObject obj, int layer)
        {
            removeObjects.Add(new IDObject(GenerateUUIDDestroyable(), obj), layer);
            obj.Destroy();
        }



        internal void Destroy(GameObject obj)
        {
            Destroy(obj, obj.Layer);
        }

        protected unsafe override void Initalize()
        {
            /* Stage s = new Stage();
             s.AddNewStageObject(new StageObject(new Vector2(0f, 100), new Vector2(40, 56), 0, "test_sp"));
             Stage.CreateFileFromStage(s);*/


            GameObject player = GameObject.CreateGameObjectSprite(new Vector2(0f, 0f), new Vector2(40f, 56f), 0f, sr.verts, "test_sp");
            player.SetLayer(4);
            objects[4].objects.Add(player);
            player.AddComponent(new Player());
            player.AddComponent(new PhysicsAffected());
            player.ignoreStageSaving = true;
            player.SetAlwaysLoad(true);
            defaultTriggerObject = player;
            
            GameObject floor = GameObject.CreateGameObjectSprite(new Vector2(0f, 200f), new Vector2(1000f, 40f), 0f, sr.verts, "black");
            floor.SetLayer(4);
            objects[4].objects.Add(floor);
            PhysicsAffected pa = new PhysicsAffected();
            floor.AddComponent(pa);
            floor.ignoreStageSaving = true;
            pa.settings.SetStatic(true);
            /*
            GameObject wall = GameObject.CreateGameObjectSprite(new Vector2(300f, 30f), new Vector2(40f, 50f), 0f, sr.verts, "test_sp");
            objects[4].objects.Add(wall);
            pa = new PhysicsAffected();
            wall.AddComponent(pa);
            pa.settings.SetStatic(true);
            */

            StageManager.LoadStagesFromFiles(this);
        }

        protected unsafe override void LoadContent()
        {
            SoundManager.InitAllSounds();

            objects.Add(0, new ObjectLayer());
            objects.Add(1, new ObjectLayer());
            objects.Add(2, new ObjectLayer());
            objects.Add(3, new ObjectLayer());
            objects.Add(4, new ObjectLayer());
            objects.Add(5, new ObjectLayer());

            UI.Add(0, new ObjectLayer());
            UI.Add(1, new ObjectLayer());
            UI.Add(2, new ObjectLayer());
            UI.Add(3, new ObjectLayer());

            TextRendering.LoadTextures();

            ResourceManager.LoadSpriteShader("../../../Rendering/Shaders/sprite.vs", "../../../Rendering/Shaders/sprite.frag", null, "sprite");

            cam = new Camera2d(Vector2.Zero, 1f);

            ResourceManager.GetSpriteShader("sprite").Use().SetInt("image", 0, false);
            ResourceManager.GetSpriteShader("sprite").SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);

            sr = new SpriteRenderer(ResourceManager.GetSpriteShader("sprite"));
            sr.initRenderData();

            TextureRegistry.LoadRegistry();
           
        }

        protected override void Render()
        {

            glClearColor(0.15f, 0.15f, 0.15f, 0);
            glClear(GL_COLOR_BUFFER_BIT);

            glBindVertexArray(sr.quadVAO);

            glDisable(GL_DEPTH_TEST);
            glEnable(GL_BLEND);

            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            foreach (ObjectLayer objlay in objects.Values)
            {
                foreach(GameObject obj in objlay.objects)
                {
                    if (obj.totalposition.X > cam.FocusPosition.X - (DisplayManager.WindowSize.X / cam.Zoom / 2f) - 300f && obj.totalposition.X < cam.FocusPosition.X + (100f + DisplayManager.WindowSize.X / cam.Zoom / 2f))
                    {
                        if (obj.totalposition.Y > cam.FocusPosition.Y - (DisplayManager.WindowSize.Y / cam.Zoom / 2f) - 300f && obj.totalposition.Y < cam.FocusPosition.Y + (100f + DisplayManager.WindowSize.Y / cam.Zoom / 2f))
                        {
                            if(obj.Loaded == false)
                            {
                                obj.OnReEnable(this);
                            }
                            obj.Loaded = true;
                            sr.DrawSprite(cam, obj, obj.texture.getTexture(), obj.color);
                        }
                        else
                        {
                            if(obj.Loaded && !obj.AlwaysLoad)
                            {
                                obj.OnDisable(this);
                                obj.Loaded = false;
                            }
                        }
                    }
                    else
                    {
                        if (obj.Loaded && !obj.AlwaysLoad)
                        {
                            obj.OnDisable(this);
                            obj.Loaded = false;
                        }
                    }
                }
            }

            foreach(ObjectLayer ol in UI.Values)
            {
                foreach (GameObject obj in ol.objects)
                {
                    sr.DrawSprite(cam, obj, obj.texture.getTexture(), obj.color, true);
                }
            }

            glBindVertexArray(0);

            Glfw.SwapBuffers(DisplayManager.Window);
        }


        protected override void Update()
        {
            foreach(KeyValuePair<IDObject, int> destroyobj in removeObjects)
            {
                objects[destroyobj.Value].objects.Remove(destroyobj.Key.gameObject);
            }

            foreach(KeyValuePair<GameObject, int> obj in createObjects)
            {
                obj.Key.SetLayer(obj.Value);
                objects[obj.Value].objects.Add(obj.Key);
            }

            createObjects.Clear();

            foreach(ObjectLayer ol in objects.Values)
            {
                foreach (GameObject obj in ol.objects)
                {
                    if(obj.Loaded || obj.AlwaysLoad)
                    {
                        obj.UpdateComponents(this);
                        obj.Update();
                        obj.Update(this);
                    }
                }
            }
        }
 
        Dictionary<GameObject, int> createObjects = new Dictionary<GameObject, int>();
        public void Instantiate(GameObject obj, int layer)
        {
            createObjects.Add(obj, layer);
        }

        static int GenerateUUIDDestroyable()
        {
            int rand = RandomF.RandomIntRange(-999999999, 999999999);
            foreach (IDObject obj in removeObjects.Keys)
            {
                if (obj.ID == rand)
                {
                    return GenerateUUIDDestroyable();
                }
            }

            return rand;
        }

       
    }
}