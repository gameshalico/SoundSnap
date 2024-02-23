using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public static class Sound
    {
        public static SoundBuilder Create(AudioClip clip)
        {
            return SoundBuilder.Create().WithClip(clip);
        }

        public static SoundBuilder CreateWithInitialValue(
            AudioClip clip,
            AudioMixerGroup outputAudioMixerGroup = null,
            bool mute = false,
            float volume = 1f,
            float pitch = 1f,
            int priority = 128,
            float panStereo = 0f,
            int startSample = 0,
            int endSample = -1,
            int loopStartSample = 0,
            int loopCount = 0,
            bool isCarryingOverLoopDifference = true,
            TimingMode timingMode = TimingMode.Immediate,
            double timingValue = 0d,
            double scheduledEndTime = -1d,
            Action<PlaybackEndType> onPlaybackEnd = null,
            Action onLoop = null)
        {
            return SoundBuilder.Create()
                .WithAllParams(clip, outputAudioMixerGroup, mute, volume, pitch, priority, panStereo, startSample,
                    endSample,
                    loopStartSample, loopCount, isCarryingOverLoopDifference, timingMode, timingValue, scheduledEndTime,
                    onPlaybackEnd, onLoop);
        }
    }
}