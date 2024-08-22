namespace SoundSnap
{
    public static class SnapBuilderExtensions
    {
        public static SnapHandle Play(this SnapBuilder snapBuilder)
        {
            return SnapPlayerProvider.Default.Play(snapBuilder.Build());
        }

        public static SnapHandle Play(this SnapBuilder snapBuilder, ISnapPlayer snapPlayer)
        {
            return snapPlayer.Play(snapBuilder.Build());
        }
    }
}