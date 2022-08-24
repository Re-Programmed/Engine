using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.GameFiles.Audio.MusicSync
{
    class MusicTick
    {
        static float BPS = 130.0f/60.0f;

        public delegate void MusicBeat(int b);
        public static MusicBeat beatTick;

        static float secondsPassed;
        static int beatsPassed;

        static bool pause = false;
        public static void PauseUpdateTick(bool p)
        {
            pause = p;
        }

        public static void ResetUpdateTick(float BPS = -1f)
        {
            if(BPS != -1f)
            {
                MusicTick.BPS = BPS;
            }

            secondsPassed = 0;
            beatsPassed = 0;
        }

        public static void UpdateTick()
        {
            if (pause) { return; }
            secondsPassed += Game.GameTime.DeltaTimeScale();

            if(secondsPassed >= 1.0f/BPS)
            {
                beatsPassed++;
                beatTick?.Invoke(beatsPassed);
                secondsPassed = 0;
            }
        }
    }
}
