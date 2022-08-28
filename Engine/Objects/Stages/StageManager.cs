using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Engine;
using Engine.Objects;
using System.Numerics;
using Engine.Objects.Components;
using Engine.GameFiles.ObjectScripts.Platforms;

namespace Engine.Objects.Stages
{
    class StageManager
    {
        static Stage[] stages;

        public static int stageId { get; private set; }  

        public static Stage currentStage { get; private set; }

        public static Dictionary<uint, GameObject> storedLoadables = new Dictionary<uint, GameObject>();

        public static List<PreregisteredObjectComponent> PreregisteredObjectComponents = new List<PreregisteredObjectComponent>();
        public static void RegisterLoadables()
        {
            storedLoadables.Add(101, GameObject.CreateGameObjectSprite(Vector2.Zero, Vector2.One * 10f, 0f, TestGame.INSTANCE.sr.verts, "test_sp"));

            RegisterPOC();

            uint i = 2;
            foreach(PreregisteredObjectComponent poc in PreregisteredObjectComponents)
            {
                foreach (GameObject g in poc.GetMyObjects())
                {
                    storedLoadables.Add(100 + i, g);
                    i++;
                }
            }

            //Unbind all POC objects that are not needed anymore.
            PreregisteredObjectComponents.Clear();
            PreregisteredObjectComponents = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Adds all PreregisteredObjectComponents to the registry.
        /// </summary>
        static void RegisterPOC()
        {
            PreregisteredObjectComponents.Add(new LerpingPlatform());
        }

        public static void LoadStagesFromFiles(TestGame game)
        {
            List<Stage> stagesTemp = new List<Stage>();
            string[] files = Directory.GetFiles("../../../stages", "*.json", SearchOption.AllDirectories);
               
            foreach(string f in files)
            {
                stagesTemp.Add(Stage.CreateStageFromFile(File.ReadAllText(f), Path.GetFileNameWithoutExtension(f).ToLower().Replace(" ", "_")));
            }

            stages = stagesTemp.ToArray();

            LoadStage(0, game);
        }

        public static bool IsMenu()
        {
            return currentStage.UI;
        }

        public static int StageCount()
        {
            if(stages == null)
            {
                return 0;
            }
            else
            {
                return stages.Length;
            }
        }

        public static void UpdateCurrentStage(TestGame game)
        {
            currentStage.stageObjects.Clear();
            foreach(KeyValuePair<int, ObjectLayer> l in game.objects)
            {
                foreach(GameObject g in l.Value.objects)
                {
                    if(!g.ignoreStageSaving)
                    {
                        currentStage.AddNewStageObject(new StageObject(g));
                    }
                }
            }

            Stage.CreateFileFromStage(currentStage);
            currentStage.LoadStage(game);
        }

        public static void SaveAllStages()
        {
            foreach(Stage s in stages)
            {
                Stage.CreateFileFromStage(s);
            }
        }

        internal static bool LoadFromStorage(LoadableStoredObject lso)
        {
            foreach(KeyValuePair<uint, GameObject> pair in storedLoadables)
            {
                if(pair.Key == (uint)lso.i)
                {
                    GameObject newObj = pair.Value.GetMemberwiseClone();

                    newObj.SetPosition(new Vector2(lso.px, lso.py));
                    newObj.SetScale(new Vector2(lso.sx, lso.sy));
                    newObj.SetLayer(lso.l);
                    newObj.SetRotation(lso.r);

                    TestGame.INSTANCE.Instantiate(newObj, lso.l);
                    return true;
                }
            }

            return false;
        }

        public static Stage GetStage(int id)
        {
            return stages[id];
        }

        public static Stage GetStage(string id)
        {
            foreach (Stage s in stages)
            {
                if (s.Name == id)
                {
                    return s;
                }
            }
            return null;
        }

        public static void LoadStage(int id, TestGame game)
        {
            stages[id].LoadStage(game);
            stageId = id;
            currentStage = stages[id];
        }

        public static void LoadStage(string id, TestGame game)
        {
            int i = 0;
            foreach(Stage s in stages)
            {
                if(s.Name == id)
                {
                    stageId = i;
                    s.LoadStage(game);
                    currentStage = s;
                    return;
                }
                i++;
            }
        }
    }
}
