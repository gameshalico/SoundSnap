using System.Collections.Generic;
using UnityEngine;

namespace SoundSnap
{
    public class SimpleSnapPlayerPool : SnapPlayerPool
    {
        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private int _initialPlayerCount;
        [SerializeField] private int _maxPlayerCount = -1;
        [SerializeField] private bool _warnIfMaxSoundPlayerCountIsExceeded = true;
        [SerializeField] private bool _warnIfOutputAudioMixerGroupIsNull = true;
        [SerializeField] private bool _isDontDestroyOnLoad = true;

        private List<SnapPlayer> _soundPlayers;
        public int PlayerCount { get; private set; }
        public int FreePlayerCount { get; private set; }

        public int MaxPlayerCount
        {
            get => _maxPlayerCount;
            set => _maxPlayerCount = value;
        }

        public bool WarnIfMaxSoundPlayerCountIsExceeded
        {
            get => _warnIfMaxSoundPlayerCountIsExceeded;
            set => _warnIfMaxSoundPlayerCountIsExceeded = value;
        }

        public bool WarnIfOutputAudioMixerGroupIsNull
        {
            get => _warnIfOutputAudioMixerGroupIsNull;
            set => _warnIfOutputAudioMixerGroupIsNull = value;
        }

        public bool IsDontDestroyOnLoad
        {
            get => _isDontDestroyOnLoad;
            set => _isDontDestroyOnLoad = value;
        }

        public AudioSource AudioSourcePrefab
        {
            get => _audioSourcePrefab;
            set => _audioSourcePrefab = value;
        }

        private void Awake()
        {
            _soundPlayers = new List<SnapPlayer>(_initialPlayerCount);
            for (var i = 0; i < _initialPlayerCount; i++)
                CreatePlayer();

            if (_isDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        public override SnapHandle Play(in SnapData snapData)
        {
            var soundPlayer = RentPlayer();

            if (soundPlayer == null)
                return SnapHandle.Invalid;

            if (_warnIfOutputAudioMixerGroupIsNull && snapData.OutputAudioMixerGroup == null)
                Debug.LogWarning(
                    "SoundSnap Warning: OutputAudioMixerGroup is null. Please set the OutputAudioMixerGroup." +
                    snapData.Clip.name, soundPlayer);

            return soundPlayer.Play(snapData);
        }

        public override void ReturnToPool(SnapPlayer snapPlayer)
        {
            FreePlayerCount++;
            snapPlayer.gameObject.SetActive(false);
        }

        public void SetAudioSourcePrefab(AudioSource audioSourcePrefab)
        {
            _audioSourcePrefab = audioSourcePrefab;
        }

        public void Initialize()
        {
        }

        public IEnumerable<SnapHandle> EnumerateActiveHandles()
        {
            foreach (var soundPlayer in _soundPlayers)
                if (!soundPlayer.IsFree)
                    yield return soundPlayer.Handle;
        }

        private SnapPlayer RentPlayer()
        {
            var soundPlayer = GetOrCreatePlayer();
            if (soundPlayer == null) return null;

            FreePlayerCount--;
            soundPlayer.gameObject.SetActive(true);

            return soundPlayer;
        }

        private SnapPlayer GetFreeSoundPlayer()
        {
            foreach (var soundPlayer in _soundPlayers)
                if (soundPlayer.IsFree)
                    return soundPlayer;

            return null;
        }


        private SnapPlayer GetOrCreatePlayer()
        {
            if (FreePlayerCount > 0) return GetFreeSoundPlayer();

            if (_maxPlayerCount < 0 || _soundPlayers.Count < _maxPlayerCount)
                return CreatePlayer();

            if (_warnIfMaxSoundPlayerCountIsExceeded)
                Debug.LogWarning(
                    "SoundSnap Warning: Max sound player count is exceeded. Please increase the max sound player count or check if there is any sound player leak.");
            return null;
        }

        private SnapPlayer CreatePlayer()
        {
            GameObject playerGameObject = null;
            if (_audioSourcePrefab)
            {
                playerGameObject = Instantiate(_audioSourcePrefab, transform).gameObject;
            }
            else
            {
                playerGameObject = new GameObject("SoundPlayer", typeof(AudioSource));
                playerGameObject.transform.SetParent(transform);
            }

            var player = playerGameObject.AddComponent<SnapPlayer>();

            player.SetPool(this);
            playerGameObject.SetActive(false);

            _soundPlayers.Add(player);
            PlayerCount++;
            FreePlayerCount++;
            return player;
        }
    }
}