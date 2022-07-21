using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Game
{
    static class GameTime
    {
        public static float DeltaTime { get; set; }

        public static float TimeScale { get; set; } = 1f;

        public static float DeltaTimeScale() { return DeltaTime * TimeScale; }

        public static float TotalElapsedSeconds { get; set; }
    }
}
