using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.Objects.UI
{
    class MenuLoader
    {
        static Menu[] menus;
        public static void LoadMenusFromFiles(TestGame game)
        {
            List<Menu> menusTemp = new List<Menu>();
            string[] files = Directory.GetFiles("../../../stages", "*.menu", SearchOption.AllDirectories);

            foreach (string f in files)
            {
                menusTemp.Add(Menu.CreateMenuFromFile(File.ReadAllText(f), Path.GetFileNameWithoutExtension(f).ToLower().Replace(" ", "_")));
            }

            menus = menusTemp.ToArray();
        }

        public static void LoadMenu(int id, TestGame game)
        {
            menus[id].LoadMenu(game);
        }

        public static void LoadMenu(string id, TestGame game)
        {
            int i = 0;
            foreach (Menu m in menus)
            {
                if (m.Name == id)
                {
                    m.LoadMenu(game);
                    return;
                }
                i++;
            }
        }
    }
}
