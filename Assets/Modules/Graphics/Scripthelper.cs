using System;
using MoonSharp.Interpreter;

namespace LuaBridge.Jlui
{
    public static class ScriptHelper
    {
        public static void RegisterSimpleFunc<T>()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T>),
                v =>
                {
                    var function = v.Function;

                    return (Func<T>)(() => function.Call().ToObject<T>());
                }
            );
        }

        public static void RegisterSimpleFunc<T1, TResult>()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T1, TResult>),
                v =>
                {
                    var function = v.Function;

                    return (Func<T1, TResult>)((T1 p1) => function.Call(p1).ToObject<TResult>());
                }
            );
        }

        public static void RegisterSimpleAction<T>()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action<T>),
                v =>
                {
                    var function = v.Function;

                    return (Action<T>)(p => function.Call(p));
                }
            );
        }

        public static void RegisterSimpleAction()
        {
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Action),
                v =>
                {
                    var function = v.Function;

                    return (Action)(() => function.Call());
                }
            );
        }
    }
}