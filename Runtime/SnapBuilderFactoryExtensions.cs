namespace SoundSnap
{
    public static class SnapBuilderFactoryExtensions
    {
        public static SnapHandle Play(this ISnapBuilderFactory snapBuilderFactory)
        {
            return snapBuilderFactory.CreateBuilder().Play();
        }

        public static SnapHandle Play(this ISnapBuilderFactory snapBuilderFactory, ISnapPlayer snapPlayer)
        {
            return snapBuilderFactory.CreateBuilder().Play(snapPlayer);
        }
    }
}