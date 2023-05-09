using System;
using UnityEngine;

namespace Utils.Unity
{
    public class EventRaiser : MonoBehaviour
    {
        public event Action Awoken, Started, ApplicationQuitted, Destroyed, OnUpArrow, OnDownArrow, OnRightArrow, OnLeftArrow;
        public event Action<bool> ApplicationPaused, Focussed;

        private void Awake()
        {
            Awoken?.Invoke();
        }

        private void Start()
        {
            Started?.Invoke();
        }

        private void OnApplicationQuit()
        {
            ApplicationQuitted?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            ApplicationPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Focussed?.Invoke(hasFocus);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnUpArrow?.Invoke();
            if (Input.GetKeyDown(KeyCode.DownArrow))
                OnDownArrow?.Invoke();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnRightArrow?.Invoke();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnLeftArrow?.Invoke();
        }
    }
}