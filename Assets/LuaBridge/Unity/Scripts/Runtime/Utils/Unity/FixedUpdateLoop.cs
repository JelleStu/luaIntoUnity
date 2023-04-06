using System;

namespace Utils.Unity
{
    public class FixedUpdateLoop : EventRaiser
    {
        public event Action FixedUpdated;

        private void FixedUpdate()
        {
            FixedUpdated?.Invoke();
        }
    }
}