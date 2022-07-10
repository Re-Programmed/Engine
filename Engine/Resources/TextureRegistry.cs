using Engine.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.Resources
{
    class TextureRegistry
    {
        const string DefaultTextureData = "../../../textures/default_data.DATA";
        public static void LoadRegistry()
        {
            string[] filedata = Directory.GetFiles("../../../textures", "*.DATA", SearchOption.AllDirectories);

            string[] files = Directory.GetFiles("../../../textures", "*.png", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                string data = DefaultTextureData;
                bool usingDefault = true;

                foreach(string data_check in filedata)
                {
                    if(Path.GetFileNameWithoutExtension(file) == Path.GetFileNameWithoutExtension(data_check))
                    {
                        data = data_check;
                        usingDefault = false;
                        break;
                    }
                }

                string datacontent = File.ReadAllText(data);
                string[] dataarray = datacontent.Split('\n');

                string name = usingDefault ? Path.GetFileNameWithoutExtension(file) : dataarray[2].Replace("NAME=", "").ToLower().Replace(" ", "_");

                ResourceManager.LoadTexture(file, dataarray[1].Contains("true"), name);
            }
        }
    }
}
