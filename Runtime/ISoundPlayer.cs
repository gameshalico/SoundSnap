using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public interface ISoundPlayer
    {
        ushort Version { get; }
        double PlayDspTime { get; }

        AudioClip Clip { get; set; }
        bool IsPlaying { get; }
        float Time { get; set; }
        bool Mute { get; set; }
        float Volume { get; set; }
        float Pitch { get; set; }
        int Priority { get; set; }
        float PanStereo { get; set; }
        int TimeSamples { get; set; }
        AudioMixerGroup OutputAudioMixerGroup { get; set; }

        int LoopCount { get; set; }
        bool IsCarryingOverLoopDifference { get; set; }
        int LoopStartSample { get; set; }
        int EndSample { get; set; }
        event Action OnLoop;
        event Action<PlaybackEndType> OnPlaybackEnd;
        void Stop();
        void Pause();
        void UnPause();
        void SetScheduledStartTime(double time);
        void SetScheduledEndTime(double time);
    }
}