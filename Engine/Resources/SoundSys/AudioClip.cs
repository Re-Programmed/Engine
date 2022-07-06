using System;
using System.Collections.Generic;
using System.Text;
using System.Media;

namespace Engine.SoundSys
{
    class AudioClip
    {
        public string id { get; private set; }
        bool disposed = false;
        SoundPlayer sound;

        public delegate void Disposed(AudioClip ac);
        public Disposed disposed_callback;

        public void Play()
        {
            if (disposed) { Console.WriteLine("Tried to play a disposed audio file!"); return; }
            sound.Stop();
            sound.Play();
        }

        public void PlayLooping()
        {
            if (disposed) { Console.WriteLine("Tried to loop play a disposed audio file!"); return; }
            sound.Stop();
            sound.PlayLooping();
        }

        public void Stop()
        {
            if (disposed) { Console.WriteLine("Tried to stop a disposed audio file!"); return; }
            sound.Stop();
        }

        public void Dispose()
        {
            if (disposed) { Console.WriteLine("Tried to dispose a disposed audio file!"); return; }
            disposed = true;
            sound.Stop();
            sound.Dispose();
            disposed_callback?.Invoke(this);
        }

        public AudioClip(string dir)
        {
            RegisterSoundFile(dir);
        }

        public void RegisterSoundFile(string dir)
        {
            id = System.IO.Path.GetFileNameWithoutExtension(dir);
            sound = new SoundPlayer(System.IO.Path.GetFullPath(dir));
        }
    }
}
