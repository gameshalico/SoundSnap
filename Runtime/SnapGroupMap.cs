using System.Collections.Generic;
using UnityEngine;

namespace SoundSnap
{
    public class SnapGroupMap<TKey>
    {
        private static SnapGroupMap<TKey> s_defaultMap;
        private readonly Dictionary<TKey, SnapGroup> _snapGroups;

        public SnapGroupMap(int capacity = 0)
        {
            _snapGroups = new Dictionary<TKey, SnapGroup>(capacity);
        }

        public static SnapGroupMap<TKey> Default => s_defaultMap ??= new SnapGroupMap<TKey>();

        public SnapGroup this[TKey key] => GetOrAddGroup(key);

        public bool TryGetGroup(TKey key, out SnapGroup snapGroup)
        {
            return _snapGroups.TryGetValue(key, out snapGroup);
        }

        public SnapGroup GetOrAddGroup(TKey key)
        {
            if (_snapGroups.TryGetValue(key, out var snapGroup))
                return snapGroup;

            snapGroup = new SnapGroup();
            _snapGroups.Add(key, snapGroup);
            return snapGroup;
        }

        public void RemoveGroup(TKey key)
        {
            if (!_snapGroups.TryGetValue(key, out var snapGroup))
                return;

            snapGroup.Clear();
            _snapGroups.Remove(key);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            s_defaultMap = null;
        }
    }
}