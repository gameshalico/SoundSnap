namespace SoundSnap
{
    public static class SoundBuilderExtensions
    {
        public static SoundHandle Play(this SoundBuilder soundBuilder)
        {
            return SoundPool.Instance.Play(soundBuilder.Build());
        }
    }
}