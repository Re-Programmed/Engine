using Engine.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.Resources
{
    class TextureRegistry
    {
        public static void LoadRegistry()
        {
            string[] filedata = Directory.GetFiles("../../../textures", "*.DATA", SearchOption.AllDirectories);

            string[] files = Directory.GetFiles("../../../textures", "*.png", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                foreach(string data in filedata)
                {
                    if(Path.GetFileNameWithoutExtension(file) == Path.GetFileNameWithoutExtension(data))
                    {
                        string datacontent = File.ReadAllText(data);
                        string[] dataarray = datacontent.Split('\n');

                        ResourceManager.LoadTexture(file, dataarray[1].Contains("true"), dataarray[2].Replace("NAME=", "").ToLower().Replace(" ", "_"));
                        break;
                    }
                }
            }
        }
    }
}
