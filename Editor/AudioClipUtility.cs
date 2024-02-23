using UnityEngine;

namespace SoundSnap.Editor
{
    public static class AudioClipUtility
    {
        public static (int startSample, int endSample) DetectSilenceSamples(AudioClip audioClip)
        {
            var samples = new float[audioClip.samples * audioClip.channels];
            if (!audioClip.GetData(samples, 0))
            {
                Debug.LogError("Failed to get data from audio clip");
                return (0, 0);
            }

            var startSample = 0;
            var endSample = samples.Length - 1;

            for (var i = 0; i < samples.Length; i++)
                if (samples[i] > 0)
                {
                    startSample = i / audioClip.channels;
                    break;
                }

            for (var i = samples.Length - 1; i >= 0; i--)
                if (samples[i] > 0)
                {
                    endSample = i / audioClip.channels;
                    break;
                }

            return (startSample, endSample);
        }
    }
}