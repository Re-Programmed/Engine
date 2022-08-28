using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Windows.Media;
using Engine.Resources.SoundSys.SoundGroups;

namespace Engine.SoundSys
{
    class AudioClip
    {
        public SGroup grouping;
        public string id { get; private set; }
        /// <summary>
        /// True if the sound is cleared from memory.
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// True if the sound is not paused or stopped.
        /// </summary>
        bool playing = false;
        /// <summary>
        /// True if the sound is not paused or playing.
        /// </summary>
        bool stopped = true;
        MediaPlayer sound;

        public delegate void Disposed(AudioClip ac);
        public Disposed disposed_callback;

        float volume = 1f;

        /// <summary>
        /// Plays this sound.
        /// </summary>
        /// <param name="continue_s">If true, the sound will continue playing from where it was last paused. If false it will restart. This param does not apply if the sound was stopped, as stopping a sound resets its playback position.</param>
        public void Play(bool continue_s = false)
        {
            if (disposed) { Console.WriteLine("Tried to play a disposed audio file!"); return; }
            if (!continue_s && !stopped)
            {
                sound.Stop();
            }

            sound.Play();
            playing = true;
            stopped = false;
        }
        
        public void SetVolume(float vol)
        {
            volume = vol;
            sound.Volume = grouping.GetVolume() * vol;
        }

        public float GetVolume()
        {
            return volume;
        }

        /// <summary>
        /// Pauses the sound.
        /// </summary>
        /// <returns>Returns true if the sound was able to be paused. I.E. it was actually playing.</returns>
        public bool Pause()
        {
            if (disposed) { Console.WriteLine("Tried to play a disposed audio file!"); return false; }
            if (!sound.CanPause || !playing || stopped) { return false; }
            sound.Pause();
            playing = false;
            stopped = false;
            return true;
        }

        public void PlayLooping()
        {
            if (disposed) { Console.WriteLine("Tried to loop play a disposed audio file!"); return; }
            sound.Stop();
            sound.Play();
            stopped = false;
            playing = true;
        }

        public void Stop()
        {
            if (disposed) { Console.WriteLine("Tried to stop a disposed audio file!"); return; }
            if(!stopped)
            {
                sound.Stop();
                playing = false;
                stopped = true;
            }
        }

        public void Dispose()
        {
            if (disposed) { Console.WriteLine("Tried to dispose a disposed audio file!"); return; }
            disposed = true;
            sound.Stop();
            sound.Close();
            sound.Freeze();
            playing = false;
            stopped = false;
            sound.MediaEnded -= OnMediaEnd;
            grouping.OnChangeCallback -= UpdateData;
            disposed_callback?.Invoke(this);
        }

        public AudioClip(string dir, SGroup group)
        {
            grouping = group;
            RegisterSoundFile(dir);

            group.OnChangeCallback += UpdateData;
        }

        void UpdateData(float volume)
        {
            sound.Volume = volume * this.volume;
        }

        public void RegisterSoundFile(string dir)
        {
            id = System.IO.Path.GetFileNameWithoutExtension(dir);
            sound = new MediaPlayer();
            sound.Open(new Uri(System.IO.Path.GetFullPath(dir)));
            sound.MediaEnded += OnMediaEnd;
        }

        void OnMediaEnd(object sender, EventArgs e)
        {
            playing = false;
            stopped = true;
        }

        public bool GetPlaying() { return playing; }
        public bool GetStopped() { return stopped; }
    }
}
