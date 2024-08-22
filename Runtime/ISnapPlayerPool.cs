namespace SoundSnap
{
    public interface ISnapPlayerPool : ISnapPlayer
    {
        public void ReturnToPool(SnapPlayer snapPlayer);
    }
}