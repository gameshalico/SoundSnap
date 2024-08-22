using UnityEngine;

namespace SoundSnap
{
    public static class SnapBuilderRandomExtensions
    {
        public static SnapBuilder SetVolumeRandom(this SnapBuilder builder, float min,
            float max)
        {
            return builder.WithVolume(Random.Range(min, max));
        }

        public static SnapBuilder SetPitchRandom(this SnapBuilder builder, float min,
            float max)
        {
            return builder.WithPitch(Random.Range(min, max));
        }
    }
}