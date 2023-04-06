using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class FloatExtensions
    {
        public static bool AlmostEqual(this float self, float other, float tolerance = float.Epsilon)
        {
            return Mathf.Abs(self - other) < tolerance;
        }

        public static bool AlmostZero(this float self, float tolerance = float.Epsilon)
        {
            return self.AlmostEqual(0, tolerance);
        }

        public static float Pythagoras(float a, float b)
        {
            return Mathf.Sqrt(a.Squared() + b.Squared());
        }

        public static float Squared(this float value)
        {
            return value * value;
        }
        
        public static float Remap (this float value, float from1, float to1, float from2, float to2) 
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static bool IsInRange(this float value, float min, float max, bool includeLimits = true)
        {
            if (includeLimits)
                return value >= min && value <= max;
            return value > min && value < max;
        }

        public static float MoveBetween(this float value, float minValue, float maxValue)
        {
            if (minValue > maxValue)
            {
                Debug.LogError($"Could not move {value} between {minValue} and {maxValue} as the boundaries aren't correct");
                return value;
            }
            var range = maxValue-minValue;
            while (value >= maxValue)
                value -= range;
            while (value < minValue)
                value += range;
            return value;
        }
        
        public static float Round(this float value, int digits)
        {
            var mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;
        }

        public static float Half(this float value) => value * 0.5f;
    }
}
