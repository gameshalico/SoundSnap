namespace SoundSnap
{
    public interface ISnapPlayer
    {
        public SnapHandle Play(in SnapData snapData);
    }
}