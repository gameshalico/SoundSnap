using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSnap
{
    public interface ISoundPool
    {
        public void ReturnToPool(SoundPlayer soundPlayer);
    }

    public class SoundPool : MonoBehaviour, ISoundPool
    {
        private static SoundPool s_instance;
        private List<SoundPlayer> _soundPlayers;
        public int PlayerCount { get; private set; }
        public int FreePlayerCount { get; private set; }

        public static SoundPool Instance
        {
            get
            {
                if (s_instance == null)
                    if (SoundSnapSettings.Instance.IsAutoGeneratePool)
                        CreateInstance();
                    else
                        throw new InvalidOperationException(
                            "SoundManager is not created. Please create SoundManager manually or set SoundKitSettings.IsAutoGenerate to true.");

                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = this;

            _soundPlayers = new List<SoundPlayer>(SoundSnapSettings.Instance.InitialSoundPlayerCount);
            for (var i = 0; i < SoundSnapSettings.Instance.InitialSoundPlayerCount; i++)
                CreatePlayer();

            if (SoundSnapSettings.Instance.IsDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            s_instance = null;
        }

        public void ReturnToPool(SoundPlayer soundPlayer)
        {
            FreePlayerCount++;
            soundPlayer.gameObject.SetActive(false);
        }

        public static IEnumerable<SoundHandle> EnumerateActiveSoundHandles()
        {
            foreach (var soundPlayer in Instance._soundPlayers)
                if (!soundPlayer.IsFree)
                    yield return soundPlayer.Handle;
        }

        public SoundHandle Play(in SoundPlayData soundPlayData)
        {
            var soundPlayer = RentPlayer();

            if (soundPlayer == null)
                return SoundHandle.Invalid;

            if (soundPlayData.OutputAudioMixerGroup == null &&
                SoundSnapSettings.Instance.WarnIfOutputAudioMixerGroupIsNull)
                Debug.LogWarning(
                    "SoundSnap Warning: OutputAudioMixerGroup is null. Please set the OutputAudioMixerGroup." +
                    soundPlayData.Clip.name, soundPlayer);

            soundPlayer.Play(soundPlayData);

            return soundPlayer.Handle;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (SoundSnapSettings.Instance.IsAutoGeneratePool && s_instance == null) CreateInstance();
        }

        private static void CreateInstance()
        {
            var _ = new GameObject("SoundPool", typeof(SoundPool));
        }

        private SoundPlayer RentPlayer()
        {
            var soundPlayer = GetOrCreatePlayer();
            if (soundPlayer == null) return null;

            FreePlayerCount--;
            soundPlayer.gameObject.SetActive(true);

            return soundPlayer;
        }

        private SoundPlayer GetFreeSoundPlayer()
        {
            foreach (var soundPlayer in _soundPlayers)
                if (soundPlayer.IsFree)
                    return soundPlayer;

            return null;
        }


        private SoundPlayer GetOrCreatePlayer()
        {
            if (FreePlayerCount > 0) return GetFreeSoundPlayer();

            if (SoundSnapSettings.Instance.MaxSoundPlayerCount < 0 ||
                _soundPlayers.Count < SoundSnapSettings.Instance.MaxSoundPlayerCount)
                return CreatePlayer();

            if (SoundSnapSettings.Instance.WarnIfMaxSoundPlayerCountIsExceeded)
                Debug.LogWarning(
                    "SoundSnap Warning: Max sound player count is exceeded. Please increase the max sound player count or check if there is any sound player leak.");
            return null;
        }

        private SoundPlayer CreatePlayer()
        {
            var soundPlayerGameObject = new GameObject("SoundPlayer", typeof(AudioSource));
            soundPlayerGameObject.transform.SetParent(transform);

            var soundPlayer = soundPlayerGameObject.AddComponent<SoundPlayer>();

            soundPlayer.SetSoundPool(this);
            soundPlayerGameObject.SetActive(false);

            _soundPlayers.Add(soundPlayer);
            PlayerCount++;
            FreePlayerCount++;
            return soundPlayer;
        }
    }
}