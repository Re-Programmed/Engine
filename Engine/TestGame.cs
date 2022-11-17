using Engine.Game;
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
using Engine.SoundSys;
using Engine.Physics;
using Engine.Objects.Stages;
using Engine.GameFiles;
using Engine.Objects.UI;
using Engine.Resources.SaveData;
using Engine.Objects.Components.UIComponents;
using Engine.Resources.AchievementsSystem;
using Engine.Input.Utils;
using Engine.GameFiles.Menus;
using Engine.GameFiles.Audio.MusicSync;
using DiscordRPC;
using Engine.GameFiles.Audio.MusicGroups;
using Engine.misc;
using Engine.GameFiles.ObjectScripts;

namespace Engine
{
    class TestGame : Game.Game
    {
        public static TestGame INSTANCE;

        public readonly bool DEVELOPER_MODE = Environment.GetCommandLineArgs().Length > 1 && Environment.GetCommandLineArgs()[1] == "Developer";

        //byte vao;
        //byte vbo;

        //Shader shader;

        public SpriteRenderer sr { get; private set; }

        protected static Dictionary<IDObject, int> removeObjects = new Dictionary<IDObject, int>();

        protected Camera2d camUI = new Camera2d(Vector2.Zero, 1f);

        static List<UpdateDependent> updateDependents = new List<UpdateDependent>();
        public static void RegisterUpdateDependent(UpdateDependent ud) { updateDependents.Add(ud); }

#pragma warning disable CS0649
        static GameObject defaultTriggerObject;
#pragma warning restore CS0649

        public static GameObject GetDefaultTriggerObject() { return defaultTriggerObject; }

        public TestGame(int initWindowWidth, int initWindowHeight, string initWindowTitle) : base(initWindowWidth, initWindowHeight, initWindowTitle)
        {

        }

        public static void Destroy(GameObject obj, int layer)
        {
            removeObjects.Add(new IDObject(GenerateUUIDDestroyable(), obj), layer);
            obj.Destroy(INSTANCE);
        }



        internal void Destroy(GameObject obj)
        {
            Destroy(obj, obj.Layer);
        }

        Achievement[] load_achievements = { new Achievement(0, "Test Achievement") };

        void RegisterAchievements()
        {
            foreach(Achievement a in load_achievements)
            {
                AchievementManager.RegisterAchievement(a);
            }

        }

        protected unsafe override void Initalize()
        {
            Engine.DiscordRPC.RPCManager.Initialize();

            AchievementManager.LoadAchievements();

            KeybindManager.LoadKeybinds();

            RegisterAchievements();

            StoredIDObject.LoadStoredObjects();

            /* Stage s = new Stage();
             s.AddNewStageObject(new StageObject(new Vector2(0f, 100), new Vector2(40, 56), 0, "test_sp"));
             Stage.CreateFileFromStage(s);*/
            
            GameObject player = GameObject.CreateGameObjectSprite(new Vector2(0f, 0f), new Vector2(40f, 56f), 0f, sr.verts, "parrot");
            player.SetLayer(4);
            objects[4].objects.Add(player);
            player.AddComponent(new Player());
            player.AddComponent(new PhysicsAffected().SetScaleMultiplier(new Vector2(0.9f, 1f), false));
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

            GameObject bg = GameObject.CreateGameObjectSprite(new Vector2(-250f, -250f), Vector2.One * 500f, 0f, sr.verts, "sky");
            bg.SetLayer(1);
            bg.ignoreStageSaving = true;
            objects[1].objects.Add(bg);

            GameObject bg2 = GameObject.CreateGameObjectSprite(new Vector2(-250f, -750f), Vector2.One * 500f, 0f, sr.verts, "sky");
            bg2.SetLayer(1);
            bg2.ignoreStageSaving = true;
            objects[1].objects.Add(bg2);

            GameObject fg = GameObject.CreateGameObjectSprite(new Vector2(50f, -1000f), new Vector2(50f, 2000f), 0f, sr.verts, "black");
            fg.SetLayer(5);
            fg.ignoreStageSaving = true;
            objects[5].objects.Add(fg);

            new ParallaxGroup(new GameObject[2] { bg, bg2 });
            new ParallaxGroup(new GameObject[1] { fg }, -1f);

            /*
            GameObject wall = GameObject.CreateGameObjectSprite(new Vector2(300f, 30f), new Vector2(40f, 50f), 0f, sr.verts, "test_sp");
            objects[4].objects.Add(wall);
            pa = new PhysicsAffected();
            wall.AddComponent(pa);
            pa.settings.SetStatic(true);
            */

            StageManager.LoadStagesFromFiles(this);
            MenuLoader.LoadMenusFromFiles(this);

            MenuLoader.LoadMenu(0, this);

            StageManager.GetStage("level").OnLoadStage += BeginLevelOne;
        }

        /// <summary>
        /// This is for testing the on load function.
        /// </summary>
        void BeginLevelOne()
        {
            DiscordRPC.RPCManager.UpdateState(new RichPresence()
            {
                Timestamps = new Timestamps(DateTime.UtcNow),
                Details = "Level One.",
                State = "Adventuring.",
                Assets = new Assets()
                {
                    LargeImageKey = "testing",
                    LargeImageText = "Adventuring",
                    SmallImageKey = "testing"
                }
            });
            //SoundManager.GetSoundById("t_city_escape").Play();
            MusicTick.ResetUpdateTick();
            Dictionary<string, PartSwitchableSong.PartType> parts = new Dictionary<string, PartSwitchableSong.PartType>();
            parts.Add("t_song_acc", PartSwitchableSong.PartType.Secondary);
            parts.Add("t_song_bass", PartSwitchableSong.PartType.Bass);
            parts.Add("t_song_chords", PartSwitchableSong.PartType.Tertiary);
            parts.Add("t_song_drums", PartSwitchableSong.PartType.Drums);
            parts.Add("t_song_lead", PartSwitchableSong.PartType.Lead);

            testSong = new PartSwitchableSong(parts);
            testSong.StartSong();
        }

        public PartSwitchableSong testSong;

        protected unsafe override void LoadContent()
        {
            INSTANCE = this;

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

            //TextRendering.LoadTextures();

            ResourceManager.LoadSpriteShader("../../../Rendering/Shaders/sprite.vs", "../../../Rendering/Shaders/sprite.frag", null, "sprite");

            cam = new Camera2d(Vector2.Zero, 1f);

            ResourceManager.GetSpriteShader("sprite").Use().SetInt("image", 0, false);
            ResourceManager.GetSpriteShader("sprite").SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);

            sr = new SpriteRenderer(ResourceManager.GetSpriteShader("sprite"));
            sr.initRenderData();

            TextureRegistry.LoadRegistry();

            StageManager.RegisterLoadables();
        }

        protected override void Render()
        {
            glClearColor(0.15f, 0.15f, 0.15f, 0);
            glClear(GL_COLOR_BUFFER_BIT);

            glBindVertexArray(sr.quadVAO);

            glDisable(GL_DEPTH_TEST);
            glEnable(GL_BLEND);

            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

            if (!StageManager.IsMenu())
            {
                foreach (ObjectLayer objlay in objects.Values)
                {
                    foreach (GameObject obj in objlay.objects)
                    {
                        if (obj.totalposition.X > cam.FocusPosition.X - (DisplayManager.WindowSize.X / cam.Zoom / 2f) - obj.scale.X && obj.totalposition.X < cam.FocusPosition.X + (obj.scale.X + DisplayManager.WindowSize.X / cam.Zoom / 2f))
                        {
                            if (obj.totalposition.Y > cam.FocusPosition.Y - (DisplayManager.WindowSize.Y / cam.Zoom / 2f) - obj.scale.Y && obj.totalposition.Y < cam.FocusPosition.Y + (obj.scale.Y + DisplayManager.WindowSize.Y / cam.Zoom / 2f))
                            {
                                if (obj.Loaded == false)
                                {
                                    obj.OnReEnable(this);
                                }
                                obj.Loaded = true;
                                sr.DrawSprite(cam, obj, obj.texture.getTexture(), obj.color);
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


        bool clearUI = false;

        public void ClearUI()
        {
            clearUI = true;
        }

        protected override void Update()
        {
            List<UpdateDependent> ud_remove = new List<UpdateDependent>();
            foreach(UpdateDependent ud in updateDependents)
            {
                if(ud == null || ud.Disabled)
                {
                    ud_remove.Add(ud);
                }

                ud.Update();
            }

            if(ud_remove.Count > 0)
            {
                foreach (UpdateDependent ud in ud_remove)
                {
                    updateDependents.Remove(ud);
                }
            }

            MusicTick.UpdateTick();

            PauseMenuManager.UpdateCheckKey(this);

            if(clearUI)
            {
                foreach (ObjectLayer ol in UI.Values)
                {
                    foreach(GameObject obj in ol.objects)
                    {
                        obj.Destroy(this);
                    }
                    ol.objects.Clear();
                }
                clearUI = false;
            }

            foreach (KeyValuePair<IDObject, int> destroyobj in removeObjects)
            {
                destroyobj.Key.gameObject.Destroy(this);
                objects[destroyobj.Value].objects.Remove(destroyobj.Key.gameObject);
            }


            foreach(KeyValuePair<GameObject, int> obj in createObjects)
            {
                obj.Key.SetLayer(obj.Value);
                if(StageManager.IsMenu())
                {
                    UI[obj.Value].objects.Add(obj.Key);
                }
                else
                {
                    objects[obj.Value].objects.Add(obj.Key);
                }
            }

            removeObjects.Clear();
            createObjects.Clear();

            if (!StageManager.IsMenu())
            {
                foreach (ObjectLayer ol in objects.Values)
                {
                    foreach (GameObject obj in ol.objects)
                    {
                        if (obj.Loaded || obj.AlwaysLoad)
                        {
                            obj.UpdateComponents(this);
                            obj.Update();
                            obj.Update(this);
                        }
                    }
                }
            }

            int c_Check = 0;
            foreach (ObjectLayer ol in UI.Values)
            {
                foreach (GameObject obj in ol.objects)
                {
                    c_Check++;
                    obj.UpdateComponents(this);
                    obj.Update();
                    obj.Update(this);
                   
                }
            }

            ButtonSelectionManager.RecieveInput(this);

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

        protected override void Close()
        {
            Engine.DiscordRPC.RPCManager.Deinitialize();

            AchievementManager.SaveAchievements();

            KeybindManager.SaveKeybinds();
        }
    }
}
