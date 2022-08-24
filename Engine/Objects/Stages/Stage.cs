using Engine;
using Engine.Objects;
using Engine.Objects.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Engine.Objects.Stages
{
    class Stage
    {
        public delegate void StageLoaded();

        /// <summary>
        /// Called on stage load. Can be used to do things on load such as play music etc.
        /// </summary>
        public StageLoaded OnLoadStage;

        public List<StageObject> stageObjects { get; set; } = new List<StageObject>();

        public List<LoadableStoredObject> stageIDObjects { get; set; } = new List<LoadableStoredObject>();


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
            Game.GameTime.TimeScale = 1f;
            foreach (ObjectLayer l in game.objects.Values)
            {
                foreach(GameObject obj in l.objects)
                {
                    if(!obj.ignoreStageSaving)
                    {
                        game.Destroy(obj);
                    }
                }
            }

            foreach (StageObject obj in stageObjects)
            {
                obj.LoadObject(game);
            }

            foreach(LoadableStoredObject lso in stageIDObjects)
            {
                if(!StageManager.LoadFromStorage(lso))
                {
                    lso.GetObject().LoadObject(game);
                }
            }

            OnLoadStage?.Invoke();
        }
    }
}
