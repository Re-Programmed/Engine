﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace Engine.Objects.UI
{
    class Menu
    {
        public List<StoredMenuObject> components { get; set; } = new List<StoredMenuObject>();

        public string Name { get; set; }

        public static Menu CreateMenuFromFile(string fileData, string fileName)
        {
            Console.WriteLine(fileData);
            Menu m = JsonSerializer.Deserialize<Menu>(fileData);
            m.Name = fileName;
            return m;
        }

        public void LoadMenu(TestGame game)
        {
            foreach (StoredMenuObject obj in components)
            {
                obj.LoadObject(game);
            }
        }
    }
}
