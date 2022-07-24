using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Resources.SoundSys.SoundGroups
{
    class SGroup
    {
        public readonly char prefix;
        public readonly string name;

        float volume = 1f;

        public delegate void OnChange(float volume);
        public OnChange OnChangeCallback;

        public SGroup(char prefix, string name)
        {
            this.prefix = prefix;
            this.name = name;
        }

        public float GetVolume()
        {
            return volume;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
            OnChangeCallback?.Invoke(volume);
        }

    }
}
