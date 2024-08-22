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

        [SerializeField] private bool _isAutoGeneratePool = true;
        [SerializeField] private SnapPlayerPool _defaultPoolPrefab;
        public bool IsAutoGeneratePool => _isAutoGeneratePool;
        public SnapPlayerPool DefaultPoolPrefab => _defaultPoolPrefab;

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