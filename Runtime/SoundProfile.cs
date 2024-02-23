using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSnap
{
    [Serializable]
    public class SoundProfile : ISoundBuilderFactory
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioMixerGroup _outputAudioMixerGroup;

        [SerializeField] private bool _mute;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private float _pitch = 1f;
        [SerializeField] private int _priority = 128;
        [SerializeField] private float _panStereo;
        [SerializeField] private int _startSample;
        [SerializeField] private int _endSample = -1;
        [SerializeField] private int _loopStartSample;
        [SerializeField] private int _loopCount;

        [SerializeField] private bool _isCarryingOverLoopDifference = true;
        [SerializeField] private double _delay;

        public SoundBuilder Create()
        {
            return Sound.CreateWithInitialValue(_clip, _outputAudioMixerGroup, _mute, _volume, _pitch,
                _priority, _panStereo, _startSample, _endSample,
                _loopStartSample, _loopCount, _isCarryingOverLoopDifference,
                _delay <= 0 ? TimingMode.Immediate : TimingMode.Delay, _delay);
        }
    }
}