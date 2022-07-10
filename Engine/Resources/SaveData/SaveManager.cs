using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Engine.Resources.SaveData
{
    static class SaveManager
    {
        const string filePathPrefix = "../../../saves/save_";
        static Dictionary<string, int?> savedValues = new Dictionary<string, int?>();

        /// <summary>
        /// Updates, or creates, a value in the savedValues array.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <param name="value">The value.</param>
        public static void SaveValue(string key, int value)
        {
            if(savedValues.ContainsKey(key))
            {
                savedValues[key] = value;
            }
            else
            {
                savedValues.Add(key, value);
            }
        }

        /// <summary>
        /// Reads the saved value based on the given key.
        /// </summary>
        /// <param name="key">The key of the value to read.</param>
        /// <param name="default_value">What to return if the requested value does not exist.</param>
        /// <returns>The value of the key given, or the default_value if none is found.</returns>
        public static int ReadValue(string key, int default_value = 0)
        {
            if(savedValues[key] == null)
            {
                return default_value;
            }
            else
            {
                return (int)savedValues[key];
            }
        }

        /// <summary>
        /// Loads the saved data from the requested slot.
        /// </summary>
        /// <param name="slot">The slot to load from, uint8.</param>
        public static void LoadSaveData(byte slot)
        {
            string filePath = filePathPrefix + slot + ".save";
            savedValues = JsonSerializer.Deserialize<Dictionary<string, int?>>(Convert.FromBase64String(File.ReadAllText(filePath)));
        }

        /// <summary>
        /// Creates a save file in the requested slot.
        /// </summary>
        /// <param name="slot">The slot to save to.</param>
        public static void CreateSaveData(byte slot, Encoding e)
        {
            string filePath = filePathPrefix + slot + ".save";
            File.WriteAllText(filePath, Convert.ToBase64String(e.GetBytes(JsonSerializer.Serialize(savedValues))));
        }
    }
}
