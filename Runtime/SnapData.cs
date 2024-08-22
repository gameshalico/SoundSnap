using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    public readonly struct SnapData
    {
        public Vector3 Position { get; init; }
        public AudioClip Clip { get; init; }
        public AudioMixerGroup OutputAudioMixerGroup { get; init; }
        public bool Mute { get; init; }
        public float Volume { get; init; }
        public float Pitch { get; init; }
        public int Priority { get; init; }
        public float PanStereo { get; init; }
        public int StartSample { get; init; }

        public bool Loop { get; init; }

        public TimingMode TimingMode { get; init; }
        public double TimingValue { get; init; }
        public double ScheduledEndTime { get; init; }

        public Action OnComplete { get; init; }
        public Action OnStart { get; init; }
        public Action OnStop { get; init; }
    }
}