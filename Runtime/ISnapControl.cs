using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public interface ISnapControl
    {
        ushort Version { get; }
        double PlayDspTime { get; }

        Vector3 Position { get; set; }
        AudioClip Clip { get; set; }
        bool IsPlaying { get; }
        float Time { get; set; }
        bool Mute { get; set; }
        float Volume { get; set; }
        float Pitch { get; set; }
        int Priority { get; set; }
        float PanStereo { get; set; }
        int TimeSamples { get; set; }
        bool Loop { get; set; }
        AudioMixerGroup OutputAudioMixerGroup { get; set; }

        event Action OnComplete;
        event Action OnStart;
        event Action OnStop;
        void Stop();
        void Pause();
        void UnPause();
        void SetScheduledStartTime(double time);
        void SetScheduledEndTime(double time);
    }
}