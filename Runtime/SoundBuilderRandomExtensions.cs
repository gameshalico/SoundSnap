using System.Linq;
using UnityEngine;

namespace SoundSnap
{
    public static class SoundBuilderRandomExtensions
    {
        public static SoundBuilder SetVolumeRandom(this SoundBuilder builder, float min,
            float max)
        {
            return builder.WithVolume(Random.Range(min, max));
        }

        public static SoundBuilder SetPitchRandom(this SoundBuilder builder, float min,
            float max)
        {
            return builder.WithPitch(Random.Range(min, max));
        }

        public static SoundBuilder AdjustVolumeForSimultaneousClips(this SoundBuilder builder)
        {
            var dspTime = builder.PlayDspTime;
            var sameClipHandlersDspTime = SoundPool.EnumerateActiveSoundHandles()
                .Where(x => x.Clip == builder.Clip && x.Volume > 0).Select(x => x.PlayDspTime);

            var volumeRate = 1f;
            foreach (var handler in sameClipHandlersDspTime)
            {
                var diff = Mathf.Abs((float)(dspTime - handler));

                if (diff < 0.025f)
                {
                    volumeRate = 0;
                    break;
                }

                if (diff < 0.05f) volumeRate *= 0.8f;
                else if (diff < 0.1f) volumeRate *= 0.9f;
            }

            return builder.WithVolume(builder.Volume * volumeRate);
        }
    }
}