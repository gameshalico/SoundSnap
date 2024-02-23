namespace SoundSnap
{
    public static class SoundPlayDataExtensions
    {
        public static SoundHandle Play(in this SoundPlayData soundPlayData)
        {
            return SoundPool.Instance.Play(in soundPlayData);
        }

        public static SoundBuilder ToBuilder(this SoundPlayData soundPlayData)
        {
            return Sound.CreateWithInitialValue(
                soundPlayData.Clip, soundPlayData.OutputAudioMixerGroup,
                soundPlayData.Mute, soundPlayData.Volume, soundPlayData.Pitch, soundPlayData.Priority,
                soundPlayData.PanStereo, soundPlayData.StartSample, soundPlayData.EndSample,
                soundPlayData.LoopStartSample, soundPlayData.LoopCount, soundPlayData.IsCarryingOverLoopDifference,
                soundPlayData.TimingMode, soundPlayData.TimingValue, soundPlayData.ScheduledEndTime,
                soundPlayData.OnPlaybackEnd, soundPlayData.OnLoop);
        }
    }
}