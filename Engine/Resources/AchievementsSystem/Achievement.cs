using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Resources.AchievementsSystem
{
    struct Achievement
    {
        byte Id;
        public readonly string Name;

        public Achievement(byte id, string name)
        {
            Id = id;
            Name = name;
        }

        public void SetId(byte value)
        {
            Id = value;
        }

        public byte GetId()
        {
            return Id;
        }
    }
}
