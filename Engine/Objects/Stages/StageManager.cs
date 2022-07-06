using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Engine;
using Engine.Objects;

namespace Engine.Objects.Stages
{
    class StageManager
    {
        static Stage[] stages;

        public static int stageId { get; private set; }  

        public static Stage currentStage { get; private set; }

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

        public static void LoadStage(int id, TestGame game)
        {
            currentStage = stages[id];
            stages[id].LoadStage(game);
            stageId = id;
        }

        public static void LoadStage(string id, TestGame game)
        {
            int i = 0;
            foreach(Stage s in stages)
            {
                if(s.Name == id)
                {
                    stageId = i;
                    currentStage = s;
                    s.LoadStage(game);
                    return;
                }
                i++;
            }
        }
    }
}
