using System;

namespace LuaBridge.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void For<T>(this T[,] array, Action<int, int> action)
        {
            for (int row = 0; row < array.RowLength(); row++)
            for (int col = 0; col < array.ColLength(); col++)
                action?.Invoke(col, row);
        }

        public static bool IsOnGrid<T>(this T[,] array, int x, int y)
        {
            return x >= 0 && y >= 0 && x < array.ColLength() && y < array.RowLength();
        }

        public static int ColLength<T>(this T[,] array)
        {
            return array.GetLength(0);
        }

        public static int RowLength<T>(this T[,] array)
        {
            return array.GetLength(1);
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            System.Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static T Random<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T[,] Copy<T>(this T[,] original)
        {
            var copy = new T[original.ColLength(), original.RowLength()];
            copy.For((col, row) => { copy[col, row] = original[col, row]; });
            return copy;
        }

        public static void Dispose<T>(this T[] array)
        {
            foreach (var disposable in array)
                if (disposable is IDisposable idsp)
                    idsp.Dispose();
        }
    }
}