using System.Linq;
using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
        }

        public static T Spawn<T>(this GameObject gameObject, Transform parent, bool resetTransform = true) where T : Object
        {
            T result = default;

            if (!gameObject)
                return result;

            GameObject spawnedObject = Object.Instantiate(gameObject, parent, true);

            if (resetTransform)
            {
                spawnedObject.transform.localScale = gameObject.transform.localScale;
                spawnedObject.transform.localPosition = gameObject.transform.localPosition;
                spawnedObject.transform.localEulerAngles = gameObject.transform.localEulerAngles;
            }

            RectTransform rectTransform = spawnedObject.GetComponent<RectTransform>();
            RectTransform originRectTransform = gameObject.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.anchorMin = originRectTransform.anchorMin;
                rectTransform.anchorMax = originRectTransform.anchorMax;
                rectTransform.offsetMin = originRectTransform.offsetMin;
                rectTransform.offsetMax = originRectTransform.offsetMax;
                rectTransform.pivot = originRectTransform.pivot;
            }

            result = spawnedObject.GetComponent<T>() ?? spawnedObject.GetComponentsInChildren<T>().FirstOrDefault();

            return result;
        }
    }
}