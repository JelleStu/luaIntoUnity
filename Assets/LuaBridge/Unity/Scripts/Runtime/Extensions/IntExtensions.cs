using UnityEngine;

namespace LuaBridge.Core.Extensions
{
    public static class IntExtensions
    {
        public static int Digits(this int n) => n == 0 ? 1 : 1 + (int)Mathf.Log10(Mathf.Abs(n));

        public static bool IsEven(this int i)
        {
            return i % 2 == 0;
        }

        public static int Squared(this int i)
        {
            return i * i;
        }
        
        public static int MoveBetween(this int value, int minValue, int maxValue)
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
        
        public static bool IsInRange(this int value, int min, int max, bool includeLimits = true)
        {
            if (includeLimits)
                return value >= min && value <= max;
            return value > min && value < max;
        }
    }
}
