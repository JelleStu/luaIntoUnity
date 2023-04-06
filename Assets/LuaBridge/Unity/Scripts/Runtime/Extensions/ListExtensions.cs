using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace LuaBridge.Core.Extensions
{
    public static class ListExtensions
    {
        private static System.Random rng = new System.Random();

        public static void QuickShuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        public static T GetRandomValue<T>(this IList<T> list)
        {
            if (list.Count > 0)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
            else
            {
                return default(T);
            }
        }

        public static List<T> ShiftLeft<T>(this List<T> list, int shiftBy = 1)
        {
            T item;
            for (int i = 0; i < shiftBy; i++)
            {
                item = list[0];
                list.RemoveAt(0);
                list.Add(item);
            }

            return list;
        }

        public static List<T> ShiftRight<T>(this List<T> list, int shiftBy = 1)
        {
            if (list.Count <= shiftBy)
            {
                return list;
            }

            var result = list.GetRange(list.Count - shiftBy, shiftBy);
            result.AddRange(list.GetRange(0, list.Count - shiftBy));
            return result;
        }

        public static void AddIfMissing<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static void RemoveIfPresent<T>(this List<T> list, T item)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
            }
        }

        public static void AddIf<T>(this List<T> list, T item, bool condition, bool checkIfMissing = false)
        {
            if (checkIfMissing && list.Contains(item))
            {
                return;
            }

            if (condition)
            {
                list.Add(item);
            }
        }

        public static void RemoveIf<T>(this List<T> list, T item, bool condition)
        {
            if (condition)
                list.RemoveIfPresent(item);
        }
        
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static List<T> ShallowCopy<T>(this List<T> list)
        {
            return list.ToList();
        }
    }
}