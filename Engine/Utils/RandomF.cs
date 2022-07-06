using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utils
{
    class RandomF
    {
        public static int RandomIntRange(int min, int max)
        {
            Random r = new Random();

            return r.Next(min, max);
        }
    }
}
