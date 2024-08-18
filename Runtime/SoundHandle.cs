using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoundSnap
{
    public readonly struct SoundHandle
    {
        public static SoundHandle Invalid => new();

        private readonly ushort _version;
        private readonly ISoundAgent _agent;

        public SoundHandle(ISoundAgent agent)
        {
            _version = agent.Version;
            _agent = agent;
        }

        public SoundHandle(SoundHandle other)
        {
            _version = other._version;
            _agent = other._agent;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            return _agent != null && _agent.Version == _version;
        }


        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Clip;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Clip = value;
            }
        }

        public bool IsPlaying
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.IsPlaying;
            }
        }

        public float Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Time;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Time = value;
            }
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Mute;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Mute = value;
            }
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Volume;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Volume = value;
            }
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Pitch;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Pitch = value;
            }
        }


        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.PanStereo;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.PanStereo = value;
            }
        }

        public int LoopCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.LoopCount;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.LoopCount = value;
            }
        }

        public int TimeSamples
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.TimeSamples;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.TimeSamples = value;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.Priority;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.Priority = value;
            }
        }

        public bool isCarryingOverLoopDifference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.IsCarryingOverLoopDifference;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.IsCarryingOverLoopDifference = value;
            }
        }

        public int LoopStartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.LoopStartSample;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.LoopStartSample = value;
            }
        }

        public int EndSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.EndSample;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _agent.EndSample = value;
            }
        }

        public event Action OnLoop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                CheckPlayer();
                _agent.OnLoop += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                CheckPlayer();
                _agent.OnLoop -= value;
            }
        }

        public event Action<PlaybackEndType> OnPlaybackEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                CheckPlayer();
                _agent.OnPlaybackEnd += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                CheckPlayer();
                _agent.OnPlaybackEnd -= value;
            }
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _agent.PlayDspTime;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Stop()
        {
            CheckPlayer();
            _agent.Stop();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            CheckPlayer();
            _agent.Pause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnPause()
        {
            CheckPlayer();
            _agent.UnPause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledStartTime(double time)
        {
            CheckPlayer();
            _agent.SetScheduledStartTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledEndTime(double time)
        {
            CheckPlayer();
            _agent.SetScheduledEndTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckPlayer()
        {
            if (_agent == null || _agent.Version != _version)
                throw new InvalidOperationException("The SoundHandle is invalid.");
        }
    }
}