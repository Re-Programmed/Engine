using Engine.SoundSys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.GameFiles.Audio.MusicGroups
{
    /// <summary>
    /// A part switchable song is a song that is made up of multiple audio files that can be toggled on and off.
    /// </summary>
    class PartSwitchableSong
    {
        public enum PartType
        {
            Lead,
            Secondary,
            Tertiary,
            Drums,
            Bass
        }

        readonly Dictionary<string, PartType> parts;

        public PartSwitchableSong(Dictionary<string, PartType> parts)
        {
            this.parts = parts;
        }

        public void PlayPart(PartType type)
        {
            foreach(KeyValuePair<string, PartType> kvp in parts)
            {
                if(kvp.Value == type)
                {
                    SoundManager.GetSoundById(kvp.Key).SetVolume(1f);
                }
            }
        }

        public void StopPart(PartType type)
        {
            foreach (KeyValuePair<string, PartType> kvp in parts)
            {
                if (kvp.Value == type)
                {
                    SoundManager.GetSoundById(kvp.Key).SetVolume(0f);
                }
            }
        }

        public void PlayAllParts()
        {
            foreach (KeyValuePair<string, PartType> kvp in parts)
            {
                SoundManager.GetSoundById(kvp.Key).SetVolume(1f);
            }
        }

        public void StopAllParts()
        {
            foreach (KeyValuePair<string, PartType> kvp in parts)
            {
                SoundManager.GetSoundById(kvp.Key).SetVolume(0f);
            }
        }

        public void StartSong()
        {
            foreach (KeyValuePair<string, PartType> kvp in parts)
            {
                AudioClip ac = SoundManager.GetSoundById(kvp.Key);
                ac.SetVolume(1f);
                ac.Play();
            }
        }

        public void StopSong()
        {
            foreach (KeyValuePair<string, PartType> kvp in parts)
            {
                SoundManager.GetSoundById(kvp.Key).Stop();
            }
        }
    }
}
