﻿using System;
using UnityEngine;

namespace SoundSnap
{
    public static class SoundPlayerUtility
    {
        public static double EvaluateDspTime(TimingMode timingMode, double timingValue)
        {
            return timingMode switch
            {
                TimingMode.Immediate => AudioSettings.dspTime,
                TimingMode.Schedule => timingValue,
                TimingMode.Delay => AudioSettings.dspTime + timingValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}