using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace SoundSnap
{
    public class SoundSnapSettings : ScriptableObject
    {
        private static SoundSnapSettings _instance;

        [Header("Pool Settings")] [SerializeField]
        private int _initialSoundPlayerCount;

        [SerializeField] private int _maxSoundPlayerCount = -1;
        [SerializeField] private bool _isAutoGeneratePool = true;
        [SerializeField] private bool _isDontDestroyOnLoad = true;

        [Header("Warnings")]
        [Tooltip("If true, a warning will be logged if the maximum sound player count is exceeded.")]
        [SerializeField]
        private bool _warnIfMaxSoundPlayerCountIsExceeded;

        [Tooltip("If true, a warning will be logged if the output audio mixer group is null.")] [SerializeField]
        private bool _warnIfOutputAudioMixerGroupIsNull = true;

        public int InitialSoundPlayerCount => _initialSoundPlayerCount;
        public int MaxSoundPlayerCount => _maxSoundPlayerCount;
        public bool IsAutoGeneratePool => _isAutoGeneratePool;
        public bool IsDontDestroyOnLoad => _isDontDestroyOnLoad;
        public bool WarnIfMaxSoundPlayerCountIsExceeded => _warnIfMaxSoundPlayerCountIsExceeded;
        public bool WarnIfOutputAudioMixerGroupIsNull => _warnIfOutputAudioMixerGroupIsNull;


        public static SoundSnapSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    var asset = PlayerSettings.GetPreloadedAssets().OfType<SoundSnapSettings>().FirstOrDefault();
                    _instance = asset != null ? asset : CreateInstance<SoundSnapSettings>();
                }

                return _instance;

#else
                if (_instance == null) _instance = CreateInstance<SoundSnapSettings>();

                return _instance;
#endif
            }
        }

        private void OnEnable()
        {
            _instance = this;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/SoundSnap Settings")]
        private static void Create()
        {
            var assetPath =
                EditorUtility.SaveFilePanelInProject($"Save {nameof(SoundSnapSettings)}", nameof(SoundSnapSettings),
                    "asset", "", "Assets");

            if (string.IsNullOrEmpty(assetPath))
                return;

            var instance = CreateInstance<SoundSnapSettings>();
            AssetDatabase.CreateAsset(instance, assetPath);
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is SoundSnapSettings);
            preloadedAssets.Add(instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            AssetDatabase.SaveAssets();
        }
#endif
    }
}