using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("")]
    public class SoundPlayer : MonoBehaviour, ISoundPlayer
    {
        public enum PlaybackState
        {
            Free,
            Playing,
            Paused
        }

        private AudioSource _audioSource;

        private Action _onLoop;
        private Action<PlaybackEndType> _onPlaybackEnd;
        private ISoundPool _soundPool;

        public PlaybackState CurrentPlaybackState
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public SoundHandle Handle => new(this);
        public bool IsFree => CurrentPlaybackState == PlaybackState.Free;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;

            CurrentPlaybackState = PlaybackState.Free;
        }

        private void Update()
        {
            CheckPlayFinished();
        }

        private void OnDestroy()
        {
            EndPlayback(PlaybackEndType.Destroy);
        }

        public int LoopCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public bool IsCarryingOverLoopDifference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public int LoopStartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public int EndSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        public ushort Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.clip;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.clip = value;
        }

        public AudioMixerGroup OutputAudioMixerGroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.outputAudioMixerGroup;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.outputAudioMixerGroup = value;
        }

        public bool IsPlaying
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.isPlaying;
        }

        public float Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.time;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.time = value;
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.mute;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.mute = value;
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.volume;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.volume = value;
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.pitch;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.pitch = value;
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.priority;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.priority = value;
        }

        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.panStereo;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.panStereo = value;
        }

        public int TimeSamples
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.timeSamples;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.timeSamples = value;
        }

        public void Stop()
        {
            EndPlayback(PlaybackEndType.Stop);
        }

        public void Pause()
        {
            _audioSource.Pause();
            CurrentPlaybackState = PlaybackState.Paused;
        }

        public void UnPause()
        {
            if (CurrentPlaybackState != PlaybackState.Paused)
                return;

            _audioSource.UnPause();
            CurrentPlaybackState = PlaybackState.Playing;
        }

        public void SetScheduledStartTime(double time)
        {
            PlayDspTime = time;
            _audioSource.SetScheduledStartTime(time);
        }

        public void SetScheduledEndTime(double time)
        {
            _audioSource.SetScheduledEndTime(time);
        }

        public event Action OnLoop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => _onLoop += value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => _onLoop -= value;
        }

        public event Action<PlaybackEndType> OnPlaybackEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => _onPlaybackEnd += value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => _onPlaybackEnd -= value;
        }

        private void PlayAudioSource(TimingMode timingMode, double timingValue)
        {
            PlayDspTime = SoundPlayerUtility.EvaluateDspTime(timingMode, timingValue);
            CurrentPlaybackState = PlaybackState.Playing;
            switch (timingMode)
            {
                case TimingMode.Immediate:
                    _audioSource.Play();
                    break;
                case TimingMode.Schedule:
                    _audioSource.PlayScheduled(timingValue);
                    break;
                case TimingMode.Delay:
                    _audioSource.PlayDelayed((float)timingValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Play(in SoundPlayData soundPlayData)
        {
            SetUp(soundPlayData);
            PlayAudioSource(soundPlayData.TimingMode, soundPlayData.TimingValue);

            if (soundPlayData.ScheduledEndTime >= 0)
                SetScheduledEndTime(soundPlayData.ScheduledEndTime);
        }

        public void SetSoundPool(ISoundPool soundPool)
        {
            _soundPool = soundPool;
        }

        private void CheckPlayFinished()
        {
            if (CurrentPlaybackState != PlaybackState.Playing)
                return;

            if (_audioSource.timeSamples < EndSample &&
                _audioSource.isPlaying)
                return;

            if (LoopCount > 0)
                LoopCount--;
            if (LoopCount == 0)
            {
                EndPlayback(PlaybackEndType.Finish);
                return;
            }

            Loop();
        }

        private void ReturnToPool()
        {
            CurrentPlaybackState = PlaybackState.Free;
            _soundPool.ReturnToPool(this);
        }

        private void Loop()
        {
            if (IsCarryingOverLoopDifference && _audioSource.isPlaying)
            {
                var gap = _audioSource.timeSamples - EndSample;
                _audioSource.timeSamples = LoopStartSample + gap;
            }
            else
            {
                _audioSource.timeSamples = LoopStartSample;
            }

            if (!_audioSource.isPlaying)
                _audioSource.Play();

            _onLoop?.Invoke();
        }

        private void SetUp(in SoundPlayData soundPlayData)
        {
            Version++;

            SetUpAudioSource(soundPlayData);
            SetUpLoopSettings(soundPlayData);
            SetUpCallbacks(soundPlayData);
        }

        private void SetUpAudioSource(in SoundPlayData soundPlayData)
        {
            _audioSource.clip = soundPlayData.Clip;
            _audioSource.outputAudioMixerGroup = soundPlayData.OutputAudioMixerGroup;
            _audioSource.mute = soundPlayData.Mute;
            _audioSource.volume = soundPlayData.Volume;
            _audioSource.pitch = soundPlayData.Pitch;
            _audioSource.priority = soundPlayData.Priority;
            _audioSource.panStereo = soundPlayData.PanStereo;
            _audioSource.timeSamples = soundPlayData.StartSample;
        }

        private void SetUpLoopSettings(in SoundPlayData soundPlayData)
        {
            LoopStartSample = soundPlayData.LoopStartSample;
            EndSample = soundPlayData.EndSample;
            LoopCount = soundPlayData.LoopCount;
            IsCarryingOverLoopDifference = soundPlayData.IsCarryingOverLoopDifference;
        }

        private void SetUpCallbacks(in SoundPlayData soundPlayData)
        {
            _onPlaybackEnd = soundPlayData.OnPlaybackEnd;
            _onLoop = soundPlayData.OnLoop;
        }


        private void EndPlayback(PlaybackEndType playEndType)
        {
            if (CurrentPlaybackState == PlaybackState.Free)
                return;

            _audioSource.Stop();
            _onPlaybackEnd?.Invoke(playEndType);
            ReturnToPool();
        }
    }
}