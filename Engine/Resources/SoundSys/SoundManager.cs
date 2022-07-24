using Engine.Resources.SoundSys.SoundGroups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.SoundSys
{
    class SoundManager
    {
        static List<AudioClip> audio = new List<AudioClip>();

        static List<AudioClip> stored_pauses = new List<AudioClip>();

        static SGroup[] sound_groups = { new SGroup('t', "Testing") };

        public static void InitAllSounds()
        {
            string[] files = Directory.GetFiles("../../../sounds", "*.wav", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                AudioClip ac = new AudioClip(file, GetSoundGroup(Path.GetFileNameWithoutExtension(file).ToCharArray()[0]));
                audio.Add(ac);
                ac.disposed_callback += OnDisposalOfClip;
            }
        }

        public static SGroup GetSoundGroup(char id)
        {
            foreach(SGroup group in sound_groups)
            {
                if(group.prefix == id)
                {
                    return group;
                }
            }

            return null;
        }

        public static void StopAllSounds()
        {
            stored_pauses.Clear();
            foreach(AudioClip ac in audio)
            {
                ac.Stop();
            }
        }

        public static void PauseAllSounds()
        {
            stored_pauses.Clear();
            foreach (AudioClip ac in audio)
            {
                if (ac.Pause()) { stored_pauses.Add(ac); }
            }
        }

        /// <summary>
        /// Resumes all songs that were previously paused using the PauseAllSounds() function.
        /// </summary>
        public static void ResumeAllSounds()
        {
            foreach(AudioClip ac in stored_pauses)
            {
                ac.Play(true);
            }

            stored_pauses.Clear();
        }

        static void OnDisposalOfClip(AudioClip clip)
        {
            if(audio.Contains(clip))
            {
                audio.Remove(clip);
            }

            if (stored_pauses.Contains(clip))
            {
                stored_pauses.Remove(clip);
            }
        }

        /// <summary>
        /// Plays the sound file with the name of the id given.
        /// </summary>
        /// <param name="id">The name of the file.</param>
        public static AudioClip GetSoundById(string id)
        {
            foreach(AudioClip ac in audio)
            {
                if(ac.id == id)
                {
                    return ac;
                }
            }

            return null;
        }
    }
}
