using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public readonly struct SnapBuilder : IDisposable
    {
        private readonly ushort _version;
        private readonly SnapBuilderBuffer _buffer;

        public static SnapBuilder Get()
        {
            return new SnapBuilder(SnapBuilderBuffer.Rent());
        }

        internal SnapBuilder(SnapBuilderBuffer buffer)
        {
            _version = buffer.Version;
            _buffer = buffer;
        }

        internal SnapBuilder WithAllParams(
            Vector3 position,
            AudioClip clip,
            AudioMixerGroup outputAudioMixerGroup = null,
            bool mute = false,
            float volume = 1f,
            float pitch = 1f,
            int priority = 128,
            float panStereo = 0f,
            int startSample = 0,
            bool loop = false,
            TimingMode timingMode = TimingMode.Immediate,
            double timingValue = 0d,
            double scheduledEndTime = -1d,
            Action onComplete = null,
            Action onStart = null,
            Action onStop = null)
        {
            ThrowIfInvalidBuffer();
            _buffer.Position = position;
            _buffer.Clip = clip;
            _buffer.OutputAudioMixerGroup = outputAudioMixerGroup;
            _buffer.Mute = mute;
            _buffer.Volume = volume;
            _buffer.Pitch = pitch;
            _buffer.Priority = priority;
            _buffer.PanStereo = panStereo;
            _buffer.StartSample = startSample;
            _buffer.Loop = loop;
            _buffer.TimingMode = timingMode;
            _buffer.TimingValue = timingValue;
            _buffer.ScheduledEndTime = scheduledEndTime;
            _buffer.OnCompleteAction = onComplete;
            _buffer.OnStartAction = onStart;
            _buffer.OnStopAction = onStop;
            return this;
        }

        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Clip;
            }
        }

        public AudioMixerGroup OutputAudioMixerGroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.OutputAudioMixerGroup;
            }
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Mute;
            }
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Volume;
            }
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Pitch;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Priority;
            }
        }

        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.PanStereo;
            }
        }

        public int StartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.StartSample;
            }
        }

        public bool Loop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.Loop;
            }
        }

        public TimingMode TimingMode
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.TimingMode;
            }
        }

        public double TimingValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.TimingValue;
            }
        }

        public double ScheduledEndTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return _buffer.ScheduledEndTime;
            }
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalidBuffer();
                return SnapSoundUtility.EvaluateDspTime(_buffer.TimingMode, _buffer.TimingValue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithPosition(Vector3 position)
        {
            ThrowIfInvalidBuffer();
            _buffer.Position = position;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithClip(AudioClip clip)
        {
            ThrowIfInvalidBuffer();
            _buffer.Clip = clip;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithOutputAudioMixerGroup(AudioMixerGroup outputAudioMixerGroup)
        {
            ThrowIfInvalidBuffer();
            _buffer.OutputAudioMixerGroup = outputAudioMixerGroup;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithMute(bool mute)
        {
            ThrowIfInvalidBuffer();
            _buffer.Mute = mute;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithVolume(float volume)
        {
            ThrowIfInvalidBuffer();
            _buffer.Volume = volume;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithPitch(float pitch)
        {
            ThrowIfInvalidBuffer();
            _buffer.Pitch = pitch;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithPriority(int priority)
        {
            ThrowIfInvalidBuffer();
            _buffer.Priority = priority;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithPanStereo(float panStereo)
        {
            ThrowIfInvalidBuffer();
            _buffer.PanStereo = panStereo;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithStartSample(int startSample)
        {
            ThrowIfInvalidBuffer();
            _buffer.StartSample = startSample;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithLoop(bool loop)
        {
            ThrowIfInvalidBuffer();
            _buffer.Loop = loop;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithImmediate()
        {
            ThrowIfInvalidBuffer();
            _buffer.TimingMode = TimingMode.Immediate;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithSchedule(double dspTime)
        {
            ThrowIfInvalidBuffer();
            _buffer.TimingMode = TimingMode.Schedule;
            _buffer.TimingValue = dspTime;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithDelay(double delay)
        {
            ThrowIfInvalidBuffer();
            _buffer.TimingMode = TimingMode.Delay;
            _buffer.TimingValue = delay;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithScheduledEndTime(double scheduledEndTime)
        {
            ThrowIfInvalidBuffer();
            _buffer.ScheduledEndTime = scheduledEndTime;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithOnComplete(Action onPlaybackEnd)
        {
            ThrowIfInvalidBuffer();
            _buffer.OnCompleteAction += onPlaybackEnd;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithOnStart(Action onStart)
        {
            ThrowIfInvalidBuffer();
            _buffer.OnStartAction += onStart;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SnapBuilder WithOnStop(Action onStop)
        {
            ThrowIfInvalidBuffer();
            _buffer.OnStopAction += onStop;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfInvalidBuffer()
        {
            if (_buffer == null || _buffer.Version != _version)
                throw new InvalidOperationException("The SoundBuilder is invalid.");
        }

        internal SnapData Build()
        {
            ThrowIfInvalidBuffer();

            if (_buffer.Clip == null)
                throw new InvalidOperationException("The AudioClip is null.");

            var data = new SnapData
            {
                Position = _buffer.Position,
                Clip = _buffer.Clip,
                OutputAudioMixerGroup = _buffer.OutputAudioMixerGroup,
                Mute = _buffer.Mute,
                Volume = _buffer.Volume,
                Pitch = _buffer.Pitch,
                Priority = _buffer.Priority,
                PanStereo = _buffer.PanStereo,
                StartSample = _buffer.StartSample,
                Loop = _buffer.Loop,
                TimingMode = _buffer.TimingMode,
                TimingValue = _buffer.TimingValue,
                ScheduledEndTime = _buffer.ScheduledEndTime,
                OnComplete = _buffer.OnCompleteAction,
                OnStart = _buffer.OnStartAction,
                OnStop = _buffer.OnStopAction
            };

            Dispose();
            return data;
        }

        public void Dispose()
        {
            if (_buffer == null)
                return;
            if (_buffer.Version != _version)
                return;

            SnapBuilderBuffer.Return(_buffer);
        }
    }
}