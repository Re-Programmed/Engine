using Engine;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Engine.Objects.Stages
{
    class Stage
    {
        public List<StageObject> stageObjects { get; set; } = new List<StageObject>();
        public string Name { get; set; }

        public bool UI { get; set; } = false;

        /// <summary>
        /// Creates a .json for this stage.
        /// </summary>
        /// <returns>The path to the file.</returns>
        public static string CreateFileFromStage(Stage stage)
        {
            string fileName = $"../../../stages/{stage.Name}.json";
            string jsonString = JsonSerializer.Serialize(stage);

            File.WriteAllText(fileName, jsonString);
            return fileName;
        }

        public static Stage CreateStageFromFile(string fileData, string fileName)
        {

                Stage st = JsonSerializer.Deserialize<Stage>(fileData);
                st.Name = fileName;
                return st;
            
        }

        public void AddNewStageObject(StageObject obj)
        {
            stageObjects.Add(obj);
        }

        public void LoadStage(TestGame game)
        {
            foreach(ObjectLayer l in game.objects.Values)
            {
                foreach(GameObject obj in l.objects)
                {
                    if(!obj.ignoreStageSaving)
                    {
                        game.Destroy(obj);
                    }
                }
            }

            foreach(StageObject obj in stageObjects)
            {
                obj.LoadObject(game);
            }
        }
    }
}
