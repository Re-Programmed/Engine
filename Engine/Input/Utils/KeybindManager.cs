using System;
using System.Collections.Generic;
using System.Text;
using Engine.Resources;

namespace Engine.Input.Utils
{
    static class KeybindManager
    {
        const string KeysFilePath = "prefs/keys.b64";

        static Dictionary<string, Keybind> keybinds;

        public static void LoadKeybinds()
        {
            keybinds = ResourceReader.ReadEncodedJSONResource<Dictionary<string, Keybind>>(KeysFilePath, Encoding.UTF8);
        }

        public static void SaveKeybinds()
        {
            ResourceReader.GenerateEncodedJSONResource(KeysFilePath, keybinds, Encoding.UTF8);
        }

        public static bool GetKeybind(string bind)
        {
            if(!keybinds.ContainsKey(bind))
            {
                return false;
            }

            return keybinds[bind].GetDown();
        }

        public static void UpdateKeybind(string bind, GLFW.Keys key)
        {
            keybinds[bind] = new Keybind(key);
        }
    }
}
