using System;

namespace Utils.Unity
{
    public class LateUpdateLoop : EventRaiser
    {
        public event Action LateUpdated;

        private void LateUpdate()
        {
            LateUpdated?.Invoke();
        }
    }
}