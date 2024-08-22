using System.Linq;
using UnityEngine;

namespace SoundSnap
{
    public static class SnapGroupExtensions
    {
        public static SnapHandle AddGroup<TKey>(this SnapHandle handle, TKey key)
        {
            SnapGroupMap<TKey>.Default[key].Add(handle);
            return handle;
        }

        public static SnapHandle AddGroup(this SnapHandle handle, SnapGroup group)
        {
            group.Add(handle);
            return handle;
        }

        public static SnapBuilder AdjustVolumeForOverlap(this SnapBuilder builder, SnapGroup group)
        {
            var dspTime = builder.PlayDspTime;
            var handlesDspTime = group.Handles
                .Where(handle => handle.IsActive() && handle.Volume > 0)
                .Select(x => x.PlayDspTime);

            var volumeRate = 1f;
            foreach (var handler in handlesDspTime)
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

        public static SnapBuilder AdjustVolumeForOverlap<TKey>(this SnapBuilder builder, TKey key)
        {
            return AdjustVolumeForOverlap(builder, SnapGroupMap<TKey>.Default[key]);
        }
    }
}