using UnityEngine;

public static class Vector2Extensions
{
    public static Vector3 ToVector3(this Vector2 vector2, float z = 0f) => new Vector3(vector2.x, vector2.y, z);

    public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(
            Mathf.Clamp(v.x, min.x, max.x),
            Mathf.Clamp(v.y, min.y, max.y));
    }

    public static Vector2 Rotate(this Vector2 vector2, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * vector2;
    }

    public static float GetAngle(this Vector2 vector)
    {
        vector = vector.normalized;
        var angle = Mathf.Atan2(vector.y, vector.x) * 180 / Mathf.PI;
        return angle;
    }
}