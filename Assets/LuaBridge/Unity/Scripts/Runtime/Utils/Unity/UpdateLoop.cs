using System;

namespace Utils.Unity
{
    public class UpdateLoop : EventRaiser
    {
        public event Action Updated;

        private void Update()
        {
            Updated?.Invoke();
        }
    }
}