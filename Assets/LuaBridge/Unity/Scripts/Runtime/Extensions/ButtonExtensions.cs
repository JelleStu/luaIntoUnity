using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LuaBridge.Core.Extensions
{
    public static class ButtonExtensions
    {
        public static bool TryAssignClick(this Button button, UnityAction callback, string logMessage = null)
        {
            if (button == null)
            {
                if (!string.IsNullOrWhiteSpace(logMessage))
                    Debug.Log(logMessage);
                return false;
            }

            button.onClick.AddListener(callback);
            return true;
        }
    }
}