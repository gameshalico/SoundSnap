using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SoundSnap
{
    public readonly struct SnapHandle : IEquatable<SnapHandle>
    {
        private readonly ISnapControl _control;

        private readonly ushort _version;

        public SnapHandle(ISnapControl control)
        {
            _version = control.Version;
            _control = control;
        }

        public SnapHandle(SnapHandle other)
        {
            _version = other._version;
            _control = other._control;
        }

        public static SnapHandle Invalid => new();

        public Vector3 Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Position;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Position = value;
            }
        }


        public AudioClip Clip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Clip;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Clip = value;
            }
        }

        public bool IsPlaying
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.IsPlaying;
            }
        }

        public float Time
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Time;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Time = value;
            }
        }

        public bool Mute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Mute;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Mute = value;
            }
        }

        public float Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Volume;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Volume = value;
            }
        }

        public float Pitch
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Pitch;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Pitch = value;
            }
        }


        public float PanStereo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.PanStereo;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.PanStereo = value;
            }
        }

        public int TimeSamples
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.TimeSamples;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.TimeSamples = value;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Priority;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Priority = value;
            }
        }

        public bool Loop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.Loop;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ThrowIfInvalid();
                _control.Loop = value;
            }
        }

        public double PlayDspTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfInvalid();
                return _control.PlayDspTime;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsActive()
        {
            return _control != null && _control.Version == _version;
        }

        public event Action OnComplete
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                ThrowIfInvalid();
                _control.OnComplete += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                ThrowIfInvalid();
                _control.OnComplete -= value;
            }
        }

        public event Action OnStart
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                ThrowIfInvalid();
                _control.OnStart += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                ThrowIfInvalid();
                _control.OnStart -= value;
            }
        }

        public event Action OnStop
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            add
            {
                ThrowIfInvalid();
                _control.OnStop += value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            remove
            {
                ThrowIfInvalid();
                _control.OnStop -= value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Stop()
        {
            ThrowIfInvalid();
            _control.Stop();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pause()
        {
            ThrowIfInvalid();
            _control.Pause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnPause()
        {
            ThrowIfInvalid();
            _control.UnPause();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledStartTime(double time)
        {
            ThrowIfInvalid();
            _control.SetScheduledStartTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScheduledEndTime(double time)
        {
            ThrowIfInvalid();
            _control.SetScheduledEndTime(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfInvalid()
        {
            if (_control == null || _control.Version != _version)
                throw new InvalidOperationException("The SoundHandle is invalid.");
        }

        public bool Equals(SnapHandle other)
        {
            return _control == other._control && _version == other._version;
        }

        public override bool Equals(object obj)
        {
            return obj is SnapHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_control, _version);
        }
    }
}