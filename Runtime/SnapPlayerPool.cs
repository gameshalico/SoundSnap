using UnityEngine;

namespace SoundSnap
{
    public abstract class SnapPlayerPool : MonoBehaviour, ISnapPlayerPool
    {
        public abstract SnapHandle Play(in SnapData snapData);
        public abstract void ReturnToPool(SnapPlayer snapPlayer);
    }
}