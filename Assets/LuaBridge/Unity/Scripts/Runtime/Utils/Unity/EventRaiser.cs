using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils.Unity
{
    public class EventRaiser : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public event Action Awoken, Started, ApplicationQuitted, Destroyed, OnUpArrow, OnDownArrow, OnRightArrow, OnLeftArrow;
        public event Action<bool> ApplicationPaused, Focussed;

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
        
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
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
            }
            Debug.Log(draggedDir);
            return draggedDir;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            GetDragDirection(dragVectorDirection);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}