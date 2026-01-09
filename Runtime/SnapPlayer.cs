using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("")]
    public class SnapPlayer : MonoBehaviour, ISnapControl, ISnapPlayer
    {
        private AudioSource _audioSource;

        private Action _onComplete;
        private Action _onStart;
        private Action _onStop;
        private ISnapPlayerPool _pool;

        public PlaybackState State
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        public SnapHandle Handle => new(this);
        public bool IsFree => State == PlaybackState.Free;


        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;

            State = PlaybackState.Free;
        }

        private void Update()
        {
            if (State == PlaybackState.Wait && _audioSource.isPlaying)
            {
                State = PlaybackState.Playing;
                _onStart?.Invoke();
            }

            CheckPlayFinished();
        }

        private void OnDestroy()
        {
            _onStop?.Invoke();
            _audioSource.Stop();
            Version++;
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

        public Vector3 Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => transform.position;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => transform.position = value;
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

        public bool Loop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _audioSource.loop;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _audioSource.loop = value;
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
            State = PlaybackState.Free;
            _onStop?.Invoke();
            _audioSource.Stop();
            ReturnToPool();
        }

        public void Pause()
        {
            _audioSource.Pause();
            State = PlaybackState.Pause;
        }

        public void UnPause()
        {
            if (State != PlaybackState.Pause)
                return;

            _audioSource.UnPause();
            State = PlaybackState.Playing;
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


        public event Action OnComplete
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => _onComplete += value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => _onComplete -= value;
        }

        public event Action OnStart
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => _onStart += value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => _onStart -= value;
        }

        public event Action OnStop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add => _onStop += value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove => _onStop -= value;
        }

        public SnapHandle Play(in SnapData snapData)
        {
            Setup(snapData);
            PlayAudioSource(snapData.TimingMode, snapData.TimingValue);

            if (snapData.ScheduledEndTime >= 0)
                SetScheduledEndTime(snapData.ScheduledEndTime);

            return Handle;
        }

        private void PlayAudioSource(TimingMode timingMode, double timingValue)
        {
            PlayDspTime = SnapSoundUtility.EvaluateDspTime(timingMode, timingValue);
            switch (timingMode)
            {
                case TimingMode.Immediate:
                    _audioSource.Play();
                    _onStart?.Invoke();
                    State = PlaybackState.Playing;
                    break;
                case TimingMode.Schedule:
                    _audioSource.PlayScheduled(timingValue);
                    State = PlaybackState.Wait;
                    break;
                case TimingMode.Delay:
                    _audioSource.PlayDelayed((float)timingValue);
                    State = PlaybackState.Wait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetPool(ISnapPlayerPool snapPlayerPool)
        {
            _pool = snapPlayerPool;
        }

        private void CheckPlayFinished()
        {
            if (State != PlaybackState.Playing)
                return;

            if (_audioSource.loop)
                return;

            if (_audioSource.isPlaying)
                return;

            State = PlaybackState.Free;
            _onComplete?.Invoke();
            _audioSource.Stop();
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _pool?.ReturnToPool(this);
            Version++;
        }

        private void Setup(in SnapData snapData)
        {
            if (State != PlaybackState.Free)
            {
                _onStop?.Invoke();
                _audioSource.Stop();
                Version++;
            }

            transform.position = snapData.Position;
            SetupAudioSource(snapData);
            SetupCallbacks(snapData);
        }

        private void SetupAudioSource(in SnapData snapData)
        {
            _audioSource.clip = snapData.Clip;
            _audioSource.loop = snapData.Loop;
            _audioSource.outputAudioMixerGroup = snapData.OutputAudioMixerGroup;
            _audioSource.mute = snapData.Mute;
            _audioSource.volume = snapData.Volume;
            _audioSource.pitch = snapData.Pitch;
            _audioSource.priority = snapData.Priority;
            _audioSource.panStereo = snapData.PanStereo;
            _audioSource.timeSamples = snapData.StartSample;
        }

        private void SetupCallbacks(in SnapData snapData)
        {
            _onComplete = snapData.OnComplete;
            _onStart = snapData.OnStart;
            _onStop = snapData.OnStop;
        }
    }
}