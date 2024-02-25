using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    internal class SoundBuilderBuffer
    {
        private static SoundBuilderBuffer s_poolRoot = new();

        private SoundBuilderBuffer _next;

        public AudioClip Clip;
        public AudioMixerGroup OutputAudioMixerGroup;
        public bool Mute;
        public float Volume = 1f;
        public float Pitch = 1f;
        public int Priority = 128;
        public float PanStereo;
        public int StartSample;
        public int EndSample = -1;

        public int LoopStartSample;
        public int LoopCount;
        public bool IsCarryingOverLoopDifference = true;

        public TimingMode TimingMode = TimingMode.Immediate;
        public double TimingValue;
        public double ScheduledEndTime = -1d;

        public Action<PlaybackEndType> OnPlaybackEnd;
        public Action OnLoop;

        public ushort Version { get; private set; }

        public static SoundBuilderBuffer Rent()
        {
            SoundBuilderBuffer result;
            if (s_poolRoot._next == null)
            {
                result = new SoundBuilderBuffer();
            }
            else
            {
                result = s_poolRoot._next;
                s_poolRoot._next = result._next;
                result._next = null;
            }

            return result;
        }

        public static void Return(SoundBuilderBuffer buffer)
        {
            buffer.Version++;

            buffer.Clip = null;
            buffer.OutputAudioMixerGroup = null;
            buffer.Mute = false;
            buffer.Volume = 1f;
            buffer.Pitch = 1f;
            buffer.Priority = 128;
            buffer.PanStereo = 0f;
            buffer.StartSample = 0;
            buffer.EndSample = -1;

            buffer.LoopStartSample = 0;
            buffer.LoopCount = 0;
            buffer.IsCarryingOverLoopDifference = true;

            buffer.TimingMode = TimingMode.Immediate;
            buffer.TimingValue = 0d;
            buffer.ScheduledEndTime = -1d;

            buffer.OnPlaybackEnd = default;
            buffer.OnLoop = default;

            if (buffer.Version != ushort.MaxValue)
            {
                buffer._next = s_poolRoot;
                s_poolRoot = buffer;
            }
        }
    }

    public readonly struct SoundBuilder : IDisposable
    {
        private readonly ushort _version;
        private readonly SoundBuilderBuffer _buffer;

        public static SoundBuilder Create()
        {
            return new SoundBuilder(SoundBuilderBuffer.Rent());
        }

        internal SoundBuilder(SoundBuilderBuffer buffer)
        {
            _version = buffer.Version;
            _buffer = buffer;
        }

        internal SoundBuilder WithAllParams(
            AudioClip clip,
            AudioMixerGroup outputAudioMixerGroup = null,
            bool mute = false,
            float volume = 1f,
            float pitch = 1f,
            int priority = 128,
            float panStereo = 0f,
            int startSample = 0,
            int endSample = -1,
            int loopStartSample = 0,
            int loopCount = 0,
            bool isCarryingOverLoopDifference = true,
            TimingMode timingMode = TimingMode.Immediate,
            double timingValue = 0d,
            double scheduledEndTime = -1d,
            Action<PlaybackEndType> onPlaybackEnd = null,
            Action onLoop = null)
        {
            CheckBuffer();
            _buffer.Clip = clip;
            _buffer.OutputAudioMixerGroup = outputAudioMixerGroup;
            _buffer.Mute = mute;
            _buffer.Volume = volume;
            _buffer.Pitch = pitch;
            _buffer.Priority = priority;
            _buffer.PanStereo = panStereo;
            _buffer.StartSample = startSample;
            _buffer.EndSample = endSample;
            _buffer.LoopStartSample = loopStartSample;
            _buffer.LoopCount = loopCount;
            _buffer.IsCarryingOverLoopDifference = isCarryingOverLoopDifference;
            _buffer.TimingMode = timingMode;
            _buffer.TimingValue = timingValue;
            _buffer.ScheduledEndTime = scheduledEndTime;
            _buffer.OnPlaybackEnd = onPlaybackEnd;
            _buffer.OnLoop = onLoop;
            return this;
        }

        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Clip;
            }
        }

        public AudioMixerGroup OutputAudioMixerGroup
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.OutputAudioMixerGroup;
            }
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Mute;
            }
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Volume;
            }
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Pitch;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Priority;
            }
        }

        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.PanStereo;
            }
        }

        public int StartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.StartSample;
            }
        }

        public int EndSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.EndSample;
            }
        }

        public int LoopStartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.LoopStartSample;
            }
        }

        public int LoopCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.LoopCount;
            }
        }

        public bool IsCarryingOverLoopDifference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.IsCarryingOverLoopDifference;
            }
        }

        public TimingMode TimingMode
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.TimingMode;
            }
        }

        public double TimingValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.TimingValue;
            }
        }

        public double ScheduledEndTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.ScheduledEndTime;
            }
        }

        public Action<PlaybackEndType> OnPlaybackEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.OnPlaybackEnd;
            }
        }

        public Action OnLoop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.OnLoop;
            }
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return SoundPlayerUtility.EvaluateDspTime(_buffer.TimingMode, _buffer.TimingValue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithClip(AudioClip clip)
        {
            CheckBuffer();
            _buffer.Clip = clip;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithOutputAudioMixerGroup(AudioMixerGroup outputAudioMixerGroup)
        {
            CheckBuffer();
            _buffer.OutputAudioMixerGroup = outputAudioMixerGroup;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithMute(bool mute)
        {
            CheckBuffer();
            _buffer.Mute = mute;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithVolume(float volume)
        {
            CheckBuffer();
            _buffer.Volume = volume;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithPitch(float pitch)
        {
            CheckBuffer();
            _buffer.Pitch = pitch;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithPriority(int priority)
        {
            CheckBuffer();
            _buffer.Priority = priority;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithPanStereo(float panStereo)
        {
            CheckBuffer();
            _buffer.PanStereo = panStereo;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithStartSample(int startSample)
        {
            CheckBuffer();
            _buffer.StartSample = startSample;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithEndSample(int endSample)
        {
            CheckBuffer();
            _buffer.EndSample = endSample;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithLoopStartSample(int loopStartSample)
        {
            CheckBuffer();
            _buffer.LoopStartSample = loopStartSample;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithLoopCount(int loopCount)
        {
            CheckBuffer();
            _buffer.LoopCount = loopCount;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithCarryingOverLoopDifference(bool isCarryingOverLoopDifference)
        {
            CheckBuffer();
            _buffer.IsCarryingOverLoopDifference = isCarryingOverLoopDifference;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder PlayImmediate()
        {
            CheckBuffer();
            _buffer.TimingMode = TimingMode.Immediate;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder Schedule(double dspTime)
        {
            CheckBuffer();
            _buffer.TimingMode = TimingMode.Schedule;
            _buffer.TimingValue = dspTime;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithDelay(double delay)
        {
            CheckBuffer();
            _buffer.TimingMode = TimingMode.Delay;
            _buffer.TimingValue = delay;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithScheduledEndTime(double scheduledEndTime)
        {
            CheckBuffer();
            _buffer.ScheduledEndTime = scheduledEndTime;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithOnPlaybackEnd(Action<PlaybackEndType> onPlaybackEnd)
        {
            CheckBuffer();
            _buffer.OnPlaybackEnd += onPlaybackEnd;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SoundBuilder WithOnLoop(Action onLoop)
        {
            CheckBuffer();
            _buffer.OnLoop += onLoop;
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckBuffer()
        {
            if (_buffer == null || _buffer.Version != _version)
                throw new InvalidOperationException("The SoundBuilder is invalid.");
        }

        internal SoundPlayData Build()
        {
            CheckBuffer();

            if (_buffer.Clip == null)
                throw new InvalidOperationException("The AudioClip is null.");

            var endSample = _buffer.EndSample < 0 ? _buffer.Clip.samples : _buffer.EndSample;

            var data = new SoundPlayData(_buffer.Clip, _buffer.OutputAudioMixerGroup, _buffer.Mute, _buffer.Volume,
                _buffer.Pitch, _buffer.Priority, _buffer.PanStereo, _buffer.StartSample, endSample,
                _buffer.LoopStartSample, _buffer.LoopCount, _buffer.IsCarryingOverLoopDifference,
                _buffer.TimingMode, _buffer.TimingValue, _buffer.ScheduledEndTime, _buffer.OnPlaybackEnd,
                _buffer.OnLoop);

            Dispose();
            return data;
        }

        public void Dispose()
        {
            if (_buffer == null)
                return;
            if (_buffer.Version != _version)
                return;

            SoundBuilderBuffer.Return(_buffer);
        }
    }
}