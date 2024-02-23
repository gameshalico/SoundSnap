using System.Threading;

namespace SoundSnap
{
    public enum SoundCancellationMode
    {
        Stop,
        Pause
    }

    public static class SoundHandleExtensions
    {
        public static SoundHandle WithCancellation(this SoundHandle soundHandle,
            CancellationToken cancellationToken, SoundCancellationMode mode = SoundCancellationMode.Stop)
        {
            cancellationToken.Register(() =>
            {
                if (soundHandle.IsActive())
                    switch (mode)
                    {
                        case SoundCancellationMode.Stop:
                            soundHandle.Stop();
                            break;
                        case SoundCancellationMode.Pause:
                            soundHandle.Pause();
                            break;
                    }
            });

            return soundHandle;
        }

        public static CancellationToken AsCancellationToken(this SoundHandle soundHandle)
        {
            var cts = new CancellationTokenSource();
            soundHandle.OnPlaybackEnd += _ => cts.Cancel();
            return cts.Token;
        }
    }
}