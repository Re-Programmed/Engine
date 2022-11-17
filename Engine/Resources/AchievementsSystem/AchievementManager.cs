using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Resources.AchievementsSystem
{
    static class AchievementManager
    {
        const string FilePath = "additional_content/achievements.b64";
        static List<AchievementStatus> achievements_status = new List<AchievementStatus>();
        static List<Achievement> achievements = new List<Achievement>();

        public static void LoadAchievements()
        {
            achievements_status = ResourceReader.ReadEncodedJSONResource<List<AchievementStatus>>(FilePath, Encoding.UTF8);

            if (achievements_status == null) { achievements_status = new List<AchievementStatus>(); }
        }

        public static void SaveAchievements()
        {
            ResourceReader.GenerateEncodedJSONResource(FilePath, achievements_status, Encoding.UTF8);
        }

        public static KeyValuePair<Achievement, AchievementStatus>? RegisterAchievement(Achievement achievement)
        {
            if(!(achievements.Count > byte.MaxValue))
            {
                byte id = Engine.Utils.Math.LimitIntToByte(achievements.Count);

                achievements.Add(achievement);
                achievement.SetId(id);

                if(!(achievements_status.Count > achievements.Count - 1))
                {
                    AchievementStatus a_as = new AchievementStatus();
                    achievements_status.Add(a_as);
                    achievement.SetId(id);
                }

                return GetAchievement(id);
            }

            return null;
        }

        public static KeyValuePair<Achievement, AchievementStatus> GetAchievement(int id)
        {
            return new KeyValuePair<Achievement, AchievementStatus>(achievements[id], achievements_status[id]);
        }

        /// <summary>
        /// Gets an achievement ID by its name. Returns -1 if no achievement has the specified name. 
        /// </summary>
        /// <param name="name">The name of the achievement to find.</param>
        /// <returns>The achievement ID.</returns>
        public static int GetAchievementByName(string name)
        {
            foreach(Achievement a in achievements)
            {
                if(a.Name == name)
                {
                    return a.GetId();
                }
            }

            return -1;
        }

        public static void GrantAchievement(int id)
        {
            GrantAchievement(GetAchievement(id));          
        }

        public static void GrantAchievement(KeyValuePair<Achievement, AchievementStatus> achievement)
        {
            achievement.Value.Grant(achievement.Key);
        }
    }
}
