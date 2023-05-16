using System;
using UnityEngine;
using UnityEngine.Events;

namespace LuaBridge.Unity.Scripts.LuaBridgeHelpers.Manager
{
    public class SwipeManager : MonoBehaviour {
        
        public float swipeThreshold = 50f;
        public float timeThreshold = 0.3f;

        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;
        public event Action OnSwipeUp;
        public event Action OnSwipeDown;

        private Vector2 fingerDown;
        private DateTime fingerDownTime;
        private Vector2 fingerUp;
        private DateTime fingerUpTime;

        private void Update () {
            if (Input.GetMouseButtonDown(0)) {
                fingerDown = Input.mousePosition;
                fingerUp = Input.mousePosition;
                fingerDownTime = DateTime.Now;
            }
            if (Input.GetMouseButtonUp(0)) {
                fingerDown = Input.mousePosition;
                fingerUpTime = DateTime.Now;
                CheckSwipe();
            }
            foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began) {
                    fingerDown = touch.position;
                    fingerUp = touch.position;
                    fingerDownTime = DateTime.Now;
                }
                if (touch.phase == TouchPhase.Ended) {
                    fingerDown = touch.position;
                    fingerUpTime = DateTime.Now;
                    CheckSwipe();
                }
            }
        }

        private void CheckSwipe() {
            float duration = (float)fingerUpTime.Subtract(fingerDownTime).TotalSeconds;
            if (duration > timeThreshold) return;

            float deltaX = fingerDown.x - fingerUp.x;
            if (Mathf.Abs(deltaX) > swipeThreshold) {
                if (deltaX > 0) {
                    OnSwipeRight.Invoke();
                    Debug.Log("right");
                } else if (deltaX < 0) {
                    OnSwipeLeft.Invoke();
                    Debug.Log("left");
                }
            }

            float deltaY = fingerDown.y - fingerUp.y;
            if (Mathf.Abs(deltaY) > swipeThreshold) {
                if (deltaY > 0) {
                    OnSwipeUp.Invoke();
                    Debug.Log("up");
                } else if (deltaY < 0) {
                    OnSwipeDown.Invoke();
                    Debug.Log("down");
                }
            }
            fingerUp = fingerDown;
        }
    }
}