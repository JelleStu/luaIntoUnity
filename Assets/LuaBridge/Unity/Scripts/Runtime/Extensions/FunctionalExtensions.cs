using System;
using System.Threading.Tasks;

namespace LuaBridge.Core.Extensions
{
    public static class FunctionalExtensions
    {
        public static TResult2 ContinueWith<T, TResult2>(this Func<T> func1, Func<T, TResult2> func2, Action<Exception> onError = null)
        {
            return func2(func1());
        }

        public static TResult2 ContinueWith<T, TResult2>(this T o, Func<T, TResult2> func2, Action<Exception> onError = null)
        {
            // try
            // {
            // }
            // catch (Exception e)
            // {
            //     if (onError == null)
            //         throw e;
            //     else
            //         onError(e);
            // }

            return func2(o);
        }
        
        public static async Task<TResult2> ContinueAwait<T, TResult2>(this Task<T> o, Func<T, TResult2> func2, Action<Exception> onError = null)
        {
            // try
            // {
            // }
            // catch (Exception e)
            // {
            //     if (onError == null)
            //         throw e;
            //     else
            //         onError(e);
            // }

            return func2(await o);
        }

        public static Func<T, TReturn2> Bind<T, TReturn2>(this T o, Func<T, TReturn2> func2)
        {
            return (x) => func2(o);
        }
        
        public static Func<T, TReturn2> Bind<T, TReturn1, TReturn2>(this Func<T, TReturn1> func1, Func<TReturn1, TReturn2> func2)
        {
            return f => func2(func1(f));
        }
        
        public static Func<T, TReturn2> Compose<T, TReturn1, TReturn2>(this Func<TReturn1, TReturn2> func1, Func<T, TReturn1> func2)
        {
            return x => func1(func2(x));
        }
    }
}