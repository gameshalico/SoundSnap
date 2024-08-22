using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    internal class SnapBuilderBuffer
    {
        private static SnapBuilderBuffer s_poolRoot = new();

        private SnapBuilderBuffer _next;

        public Vector3 Position;
        public AudioClip Clip;
        public AudioMixerGroup OutputAudioMixerGroup;
        public bool Mute;
        public float Volume = 1f;
        public float Pitch = 1f;
        public int Priority = 128;
        public float PanStereo;
        public int StartSample;

        public bool Loop;

        public TimingMode TimingMode = TimingMode.Immediate;
        public double TimingValue;
        public double ScheduledEndTime = -1d;

        public Action OnCompleteAction;
        public Action OnStartAction;
        public Action OnStopAction;

        public ushort Version { get; private set; }

        public static SnapBuilderBuffer Rent()
        {
            SnapBuilderBuffer result;
            if (s_poolRoot._next == null)
            {
                result = new SnapBuilderBuffer();
            }
            else
            {
                result = s_poolRoot;
                s_poolRoot = s_poolRoot._next;
                result._next = null;
            }

            return result;
        }

        public static void Return(SnapBuilderBuffer buffer)
        {
            buffer.Version++;

            buffer.Position = Vector3.zero;
            buffer.Clip = null;
            buffer.OutputAudioMixerGroup = null;
            buffer.Mute = false;
            buffer.Volume = 1f;
            buffer.Pitch = 1f;
            buffer.Priority = 128;
            buffer.PanStereo = 0f;
            buffer.StartSample = 0;

            buffer.Loop = false;

            buffer.TimingMode = TimingMode.Immediate;
            buffer.TimingValue = 0d;
            buffer.ScheduledEndTime = -1d;

            buffer.OnCompleteAction = null;
            buffer.OnStartAction = null;
            buffer.OnStopAction = null;

            if (buffer.Version != ushort.MaxValue)
            {
                buffer._next = s_poolRoot;
                s_poolRoot = buffer;
            }
        }
    }
}