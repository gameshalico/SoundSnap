namespace SoundSnap
{
    public static class SnapDataExtensions
    {
        public static SnapHandle Play(in this SnapData snapData, ISnapPlayer snapPlayer)
        {
            return snapPlayer.Play(snapData);
        }

        public static SnapHandle Play(in this SnapData snapData)
        {
            return SnapPlayerProvider.Default.Play(snapData);
        }

        public static SnapBuilder ToBuilder(this SnapData snapData)
        {
            return SnapBuilder.Get().WithAllParams(
                snapData.Position, snapData.Clip, snapData.OutputAudioMixerGroup,
                snapData.Mute, snapData.Volume, snapData.Pitch, snapData.Priority,
                snapData.PanStereo, snapData.StartSample, snapData.Loop,
                snapData.TimingMode, snapData.TimingValue, snapData.ScheduledEndTime,
                snapData.OnComplete, snapData.OnStart, snapData.OnStop);
        }
    }
}