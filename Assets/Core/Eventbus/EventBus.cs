using UnityEngine;

namespace Luncay.Core.EventBus
{
    public class EventBus
    {
        public void Publish(string message)
        {
            Debug.Log(message);
        }
    }
}