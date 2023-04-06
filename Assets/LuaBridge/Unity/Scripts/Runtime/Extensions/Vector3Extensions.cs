using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Clamp(this Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(
                Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z));
        }

        public static Vector3 Change(this Vector3 vector, float x = float.MinValue, float y = float.MinValue, float z = float.MinValue)
        {
            var result = vector;
            if (x != float.MinValue) result.x = x;
            if (y != float.MinValue) result.y = y;
            if (z != float.MinValue) result.z = z;
            return result;
        }
    }
}