using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace LuaBridge.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T GetRandom<T>()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var values = Enum.GetValues(typeof(T));
            return (T) values.GetValue(random.Next(values.Length));
        }

        public static T GetHighestValue<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Max();
        }

        public static T GetLowestValue<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Min();
        }

        public static bool TryToEnum<T>(this int i, out T result) where T : struct, IConvertible
        {
            if (Enum.IsDefined(typeof(T), i))
            {
                result = (T) (object) i;
                return true;
            }

            result = default;
            return false;
        }

        public static T ToEnum<T>(this int i) where T : struct, IConvertible
        {
            if (i.TryToEnum(out T result))
                return result;
            throw new ArgumentOutOfRangeException($"There is no value defined for {i} in {typeof(T)}");
        }

        public static bool TryToEnum<T>(this string s, out T result, bool ignoreCase = false) where T : struct, IConvertible
        {
            return int.TryParse(s, out int i) ? i.TryToEnum(out result) : Enum.TryParse(s, ignoreCase, out result);
        }

        public static T ToEnum<T>(this string s, bool ignoreCase = false) where T : struct, IConvertible
        {
            if (s.TryToEnum(out T result, ignoreCase))
                return result;
            throw new ArgumentOutOfRangeException($"There is no value defined for {s} in {typeof(T)}");
        }
        
        public static bool IsAnyEnumValueEqual<T>(this T value, params T[] choices) where T : Enum
        {
            return choices.Contains(value);
        }
    }

    public class Enum<T> where T : struct, IConvertible
    {
        public static void Foreach(Action<T> action)
        {
            foreach (T value in (T[]) Enum.GetValues(typeof(T)))
                action?.Invoke(value);
        }
    }
}