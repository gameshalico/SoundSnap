using UnityEngine;

namespace SoundSnap
{
    public static class SnapPlayerProvider
    {
        public static ISnapPlayer Default { get; private set; }

        public static void SetDefaultPlayer(ISnapPlayer pool)
        {
            Default = pool;
        }

        private static void CreateInstance()
        {
            if (SoundSnapSettings.Instance.DefaultPoolPrefab != null)
            {
                Default = Object.Instantiate(SoundSnapSettings.Instance.DefaultPoolPrefab);
                return;
            }

            var obj = new GameObject("SnapPool");
            Default = obj.AddComponent<SimpleSnapPlayerPool>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            if (SoundSnapSettings.Instance.IsAutoGeneratePool) CreateInstance();
            else Default = null;
        }
    }
}