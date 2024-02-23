using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public enum TimingMode
    {
        Immediate,
        Delay,
        Schedule
    }

    public enum PlaybackEndType
    {
        Stop,
        Finish,
        Destroy
    }

    public readonly struct SoundPlayData
    {
        public SoundPlayData(
            AudioClip clip,
            AudioMixerGroup outputAudioMixerGroup,
            bool mute,
            float volume,
            float pitch,
            int priority,
            float panStereo,
            int startSample,
            int endSample,
            int loopStartSample,
            int loopCount,
            bool isCarryingOverLoopDifference,
            TimingMode timingMode,
            double timingValue,
            double scheduledEndTime,
            Action<PlaybackEndType> onPlaybackEnd,
            Action onLoop)
        {
            Clip = clip;
            OutputAudioMixerGroup = outputAudioMixerGroup;
            Mute = mute;
            Volume = volume;
            Pitch = pitch;
            Priority = priority;
            PanStereo = panStereo;
            StartSample = startSample;
            EndSample = endSample;
            LoopStartSample = loopStartSample;
            LoopCount = loopCount;
            IsCarryingOverLoopDifference = isCarryingOverLoopDifference;
            TimingMode = timingMode;
            TimingValue = timingValue;
            ScheduledEndTime = scheduledEndTime;
            OnPlaybackEnd = onPlaybackEnd;
            OnLoop = onLoop;
        }

        public readonly AudioClip Clip;
        public readonly AudioMixerGroup OutputAudioMixerGroup;
        public readonly bool Mute;
        public readonly float Volume;
        public readonly float Pitch;
        public readonly int Priority;
        public readonly float PanStereo;
        public readonly int StartSample;
        public readonly int EndSample;
        public readonly int LoopStartSample;
        public readonly int LoopCount;
        public readonly bool IsCarryingOverLoopDifference;

        public readonly TimingMode TimingMode;
        public readonly double TimingValue;
        public readonly double ScheduledEndTime;

        public readonly Action<PlaybackEndType> OnPlaybackEnd;
        public readonly Action OnLoop;
    }
}