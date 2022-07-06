using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.SoundSys
{
    class SoundManager
    {
        static List<AudioClip> audio = new List<AudioClip>();

        public static void InitAllSounds()
        {

            string[] files = Directory.GetFiles("../../../sounds", "*.wav", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                AudioClip ac = new AudioClip(file);
                audio.Add(ac);
                ac.disposed_callback += OnDisposalOfClip;
            }
        }

        static void OnDisposalOfClip(AudioClip clip)
        {
            if(audio.Contains(clip))
            {
                audio.Remove(clip);
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
