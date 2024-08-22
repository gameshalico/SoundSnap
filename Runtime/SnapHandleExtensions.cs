using System;
using System.Threading;
#if SOUNDSNAP_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace SoundSnap
{
    public enum SnapCancellationMode
    {
        None,
        Stop,
        Pause
    }

    public static class SnapHandleExtensions
    {
        public static SnapHandle WithCancellation(this SnapHandle self,
            CancellationToken cancellationToken, SnapCancellationMode cancellationMode = SnapCancellationMode.None)
        {
            cancellationToken.Register(() =>
            {
                if (self.IsActive())
                    switch (cancellationMode)
                    {
                        case SnapCancellationMode.Stop:
                            self.Stop();
                            break;
                        case SnapCancellationMode.Pause:
                            self.Pause();
                            break;
                    }
            });

            return self;
        }

#if SOUNDSNAP_UNITASK_SUPPORT
        public static async UniTask ToUniTask(this SnapHandle self, CancellationToken cancellationToken = default,
            SnapCancellationMode cancellationMode = SnapCancellationMode.None)
        {
            try
            {
                await UniTask.WaitWhile(self.IsActive, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException e)
            {
                if (self.IsActive())
                    switch (cancellationMode)
                    {
                        case SnapCancellationMode.Stop:
                            self.Stop();
                            break;
                        case SnapCancellationMode.Pause:
                            self.Stop();
                            break;
                    }

                throw;
            }
        }
#endif
    }
}