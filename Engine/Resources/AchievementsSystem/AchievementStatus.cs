using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Resources.AchievementsSystem
{
    class AchievementStatus
    {
        public byte Id { get; set; }
        public bool Complete { get; set; } = false;

        public void Grant(Achievement achievement)
        {
            Complete = true;
            Console.WriteLine("You got the " + achievement.Name + " achievement.");
        }
    }
}
