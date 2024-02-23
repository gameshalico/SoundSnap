namespace SoundSnap
{
    public static class SoundBuilderFactoryExtensions
    {
        public static SoundHandle Play(this ISoundBuilderFactory soundBuilderFactory)
        {
            return soundBuilderFactory.Create().Play();
        }
    }
}