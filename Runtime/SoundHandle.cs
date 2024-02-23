using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoundSnap
{
    public readonly struct SoundHandle
    {
        public static SoundHandle Invalid => new();

        private readonly ushort _version;
        private readonly ISoundPlayer _player;

        public SoundHandle(ISoundPlayer player)
        {
            _version = player.Version;
            _player = player;
        }

        public SoundHandle(SoundHandle other)
        {
            _version = other._version;
            _player = other._player;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            return _player != null && _player.Version == _version;
        }


        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Clip;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Clip = value;
            }
        }

        public bool IsPlaying
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.IsPlaying;
            }
        }

        public float Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Time;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Time = value;
            }
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Mute;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Mute = value;
            }
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Volume;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Volume = value;
            }
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Pitch;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Pitch = value;
            }
        }


        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.PanStereo;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.PanStereo = value;
            }
        }

        public int LoopCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.LoopCount;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.LoopCount = value;
            }
        }

        public int TimeSamples
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.TimeSamples;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.TimeSamples = value;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.Priority;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.Priority = value;
            }
        }

        public bool isCarryingOverLoopDifference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.IsCarryingOverLoopDifference;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.IsCarryingOverLoopDifference = value;
            }
        }

        public int LoopStartSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.LoopStartSample;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.LoopStartSample = value;
            }
        }

        public int EndSample
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.EndSample;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckPlayer();
                _player.EndSample = value;
            }
        }

        public event Action OnLoop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                CheckPlayer();
                _player.OnLoop += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                CheckPlayer();
                _player.OnLoop -= value;
            }
        }

        public event Action<PlaybackEndType> OnPlaybackEnd
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                CheckPlayer();
                _player.OnPlaybackEnd += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                CheckPlayer();
                _player.OnPlaybackEnd -= value;
            }
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckPlayer();
                return _player.PlayDspTime;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Stop()
        {
            CheckPlayer();
            _player.Stop();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            CheckPlayer();
            _player.Pause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnPause()
        {
            CheckPlayer();
            _player.UnPause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledStartTime(double time)
        {
            CheckPlayer();
            _player.SetScheduledStartTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledEndTime(double time)
        {
            CheckPlayer();
            _player.SetScheduledEndTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void CheckPlayer()
        {
            if (_player == null || _player.Version != _version)
                throw new InvalidOperationException("The SoundHandle is invalid.");
        }
    }
}