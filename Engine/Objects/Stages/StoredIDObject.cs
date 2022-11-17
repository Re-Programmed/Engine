using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Stages
{
    class StoredIDObject
    {
        const string StoredPath = "objects/stored.b64";

        static List<StoredIDObject> storedIDObjects;
        public int id { get; set; }
        public StageObject obj { get; set; }

        static int GenerateID()
        {
            return storedIDObjects.Count;
        }

        public StoredIDObject()
        {

        }

        public StoredIDObject(StageObject obj)
        {
            this.obj = obj;
            id = GenerateID();
        }

        public static void LoadStoredObjects()
        {
            storedIDObjects = Resources.ResourceReader.ReadEncodedJSONResource<List<StoredIDObject>>(StoredPath, Encoding.UTF8);
        }

        public static StoredIDObject GetObject(int i)
        {
            foreach(StoredIDObject obj in storedIDObjects)
            {
                if(obj.id == i)
                {
                    return obj;
                }
            }

            return null;
        }
    }
}
