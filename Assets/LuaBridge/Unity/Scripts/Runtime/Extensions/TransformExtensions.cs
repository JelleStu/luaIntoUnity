using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LuaBridge.Core.Extensions
{
    public static class TransformExtensions
    {
        public static void RemoveAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                Object.Destroy(transform.GetChild(i).gameObject);
        }

        /// <summary>
        /// Executes an action for each direct child. NOT NESTED!
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="action"></param>
        public static void ForEachChild(this Transform transform, Action<Transform> action)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                action?.Invoke(transform.GetChild(i));
        }

        public static void ForcePositiveScale(this Transform transform)
        {
            if (transform.lossyScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            if (transform.lossyScale.y < 0)
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            if (transform.lossyScale.z < 0)
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
        }
    }
}