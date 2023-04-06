using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LuaBridge.Core.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static IEnumerable<T> GetComponentsInDirectChildren<T>(this MonoBehaviour monoBehaviour)
        {
            List<T> result = new List<T>();
            foreach (Transform t in monoBehaviour.transform)
            {
                T c = t.GetComponents<T>().FirstOrDefault();
                if (c != null) result.Add(c);
            }

            return result;
        }

        public static T GetComponentInDirectChildren<T>(this MonoBehaviour monoBehaviour)
        {
            return GetComponentsInDirectChildren<T>(monoBehaviour).FirstOrDefault();
        }

        public static T GetComponentInChildren<T>(this MonoBehaviour monoBehaviour, bool includeInactive = false, bool includeSelf = true) where T : Component
        {
            return includeSelf
                ? monoBehaviour.GetComponentInChildren<T>(includeInactive)
                : monoBehaviour.GetComponentsInChildren<T>(includeInactive).FirstOrDefault(c => c.transform.GetInstanceID() != monoBehaviour.transform.GetInstanceID());
        }

        public static Component GetAnyComponentInChildren(this Component component, out Type componentType, Type[] types, bool includeInactive = false)
        {
            Component result = null;
            foreach (Type t in types)
            {
                if ((result = component.GetComponentInChildren(t, includeInactive)) == null)
                    continue;

                componentType = t;
                return result;
            }

            componentType = null;
            return null;
        }

        public static List<T> GetComponentsInChildren<T>(this MonoBehaviour monoBehaviour, bool includeInactive = false, bool includeSelf = true) where T : Component
        {
            return includeSelf
                ? monoBehaviour.GetComponentsInChildren<T>(includeInactive).ToList()
                : monoBehaviour.GetComponentsInChildren<T>(includeInactive).Where(c => c.transform.GetInstanceID() != monoBehaviour.transform.GetInstanceID()).ToList();
        }

        public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.gameObject.TryGetComponent(out T component) ? component : monoBehaviour.gameObject.AddComponent<T>();
        }

        /// <summary>
        /// This version is not pauseble as it uses a coroutine to delay the execution.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static Coroutine ExecuteDelayed(this MonoBehaviour self, Action action, float delay)
        {
            return self.StartCoroutine(ExecuteDelayed(action, delay));
        }

        public static void ExecuteNextFrame(this MonoBehaviour self, Action action)
        {
            self.StartCoroutine(ExecuteNextFrame(action));
        }

        public static IEnumerator ExecuteDelayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private static IEnumerator ExecuteNextFrame(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        public static Coroutine ToggleImages(this MonoBehaviour behaviour, Image imageComponent, Sprite[] images, float delay = 1f)
        {
            if (imageComponent == null || images == null || images.Length == 0)
                return null;
            return behaviour.StartCoroutine(ToggleImages(imageComponent, images, delay));
        }

        private static IEnumerator ToggleImages(Image imageComponent, Sprite[] images, float delay)
        {
            int counter = 0;
            while (imageComponent.gameObject.activeInHierarchy)
            {
                imageComponent.sprite = images[counter];
                yield return new WaitForSeconds(delay);
                counter++;
                if (counter >= images.Length)
                    counter = 0;
            }
        }
    }
}