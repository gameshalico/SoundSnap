using System;
using System.Collections.Generic;

namespace SoundSnap
{
    public class SnapGroup
    {
        private readonly Dictionary<SnapHandle, Action> _snapHandleRegistrations;

        public SnapGroup(int capacity = 0)
        {
            _snapHandleRegistrations = new Dictionary<SnapHandle, Action>(capacity);
        }

        public IEnumerable<SnapHandle> Handles => _snapHandleRegistrations.Keys;

        public int Count => _snapHandleRegistrations.Count;

        public void Clear()
        {
            foreach (var snapHandle in _snapHandleRegistrations.Keys)
            {
                var removeAction = _snapHandleRegistrations[snapHandle];
                snapHandle.OnStop -= removeAction;
                snapHandle.OnComplete -= removeAction;
            }

            _snapHandleRegistrations.Clear();
        }

        public void Add(SnapHandle snapHandle)
        {
            if (!snapHandle.IsActive())
                throw new InvalidOperationException("SnapHandle is not active");
            if (_snapHandleRegistrations.ContainsKey(snapHandle))
                return;

            var removeAction = new Action(() => _snapHandleRegistrations.Remove(snapHandle));
            _snapHandleRegistrations.Add(snapHandle, removeAction);

            snapHandle.OnStop += removeAction;
            snapHandle.OnComplete += removeAction;
        }

        public void Remove(SnapHandle snapHandle)
        {
            if (!_snapHandleRegistrations.ContainsKey(snapHandle))
                return;

            _snapHandleRegistrations.Remove(snapHandle);

            var removeAction = _snapHandleRegistrations[snapHandle];
            snapHandle.OnStop -= removeAction;
            snapHandle.OnComplete -= removeAction;
        }
    }
}