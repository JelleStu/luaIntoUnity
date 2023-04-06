using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LuaBridge.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        public static T GetRandom<T>(this IEnumerable<T> array)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            IEnumerable<T> enumerable = array.GetClean() as T[] ?? array.GetClean().ToArray();
            if (array == null || !enumerable.Any()) return default(T);
            return enumerable.ElementAtOrDefault(random.Next(0, enumerable.Count()));
        }

        public static IEnumerable<T> GetClean<T>(this IEnumerable<T> array)
        {
            List<T> result = new List<T>();

            foreach (T item in array.Where(item => item != null))
            {
                try
                {
                    Type testType = item.GetType();
                    if (testType.IsSubclassOf(typeof(Component)))
                    {
                        Transform test = (item as Component).transform;
                    }

                    result.Add(item);
                }
                catch (Exception)
                {
                    // MUST BE EMPTY!
                }
            }

            return result;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return source;
            foreach (var sourceItem in source)
                action(sourceItem);

            return source;
        }

        public static List<List<T>> Partition<T>(this IEnumerable<T> list, int size)
        {
            var me = list.ToList();
            var result = new List<List<T>>();
            for (int i = 0; i < me.Count; i += size)
                result.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
            return result;
        }

        public static bool TryGet<T>(this IEnumerable<T> collection, Predicate<T> filter, out T result)
        {
            result = default;
            if (collection == null)
                return false;
            result = collection.FirstOrDefault(element => filter(element));
            return result != null;
        }
    }
}