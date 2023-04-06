using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LuaBridge.Core.Utils.Threading
{
    public static class Dispatcher
    {
        // [DebuggerNonUserCode]
        // private readonly struct SynchronizationContextAwaiter : ICriticalNotifyCompletion
        // {
        //     private static readonly SendOrPostCallback _postCallback = state => ((Action)state)();
        //
        //     private readonly SynchronizationContext _context;
        //
        //     internal SynchronizationContextAwaiter(SynchronizationContext context)
        //     {
        //         _context = context;
        //     }
        //
        //     public bool IsCompleted => _context == SynchronizationContext.Current || !Application.isPlaying;
        //
        //     public void OnCompleted(Action continuation) => _context.Post(_postCallback, continuation);
        //     public void UnsafeOnCompleted(Action continuation)
        //     {
        //         _context.Post(_postCallback, continuation);
        //     }
        //
        //     public void GetResult()
        //     {
        //     }
        //
        //     internal SynchronizationContextAwaiter GetAwaiter()
        //     {
        //         return this;
        //     }
        // }
        // private struct SwitchToMainThreadAwaitable
        // {
        //     internal Awaiter GetAwaiter() => new Awaiter();
        //
        //     internal struct Awaiter : INotifyCompletion
        //     {
        //         public bool IsCompleted
        //         {
        //             get
        //             {
        //                 var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        //                 if (MainThreadId == currentThreadId)
        //                 {
        //                     return true; // run immediate.
        //                 }
        //                 else
        //                 {
        //                     return false; // register continuation.
        //                 }
        //             }
        //         }
        //
        //         public void GetResult()
        //         {
        //         }
        //
        //         public void OnCompleted(Action continuation)
        //         {
        //             continuation?.Invoke();
        //         }
        //     }
        // }

        private static UpdateLoop _update;
        private static ConcurrentQueue<Action> _actionQueue;
        private static int? MainThreadId;
        private static SynchronizationContext UnityContext;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Start()
        {
            if (_update != null)
                return;
            MainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            UnityContext = SynchronizationContext.Current;
            _actionQueue = new ConcurrentQueue<Action>();
            _update = new GameObject("UpdateLoop").AddComponent<UpdateLoop>();
            _update.Destroyed += UpdateOnDestroyed_Handler;
            _update.Updated += Updated_Handler;
        }

        public static void ToMain(Action action)
        {
            if (action == null)
                return;
            _actionQueue.Enqueue(action);
        }

        public static async Task ToMain()
        {
            // await new SwitchToMainThreadAwaitable();
            // await new SynchronizationContextAwaiter(UnityContext).GetAwaiter();
        }

        private static void CleanUp()
        {
            _update.Updated -= Updated_Handler;
            _update.Destroyed -= UpdateOnDestroyed_Handler;
            _actionQueue.Clear();
        }

        private static void ProcessQueue()
        {
            if (_actionQueue.IsEmpty)
                return;

            try
            {
                if (_actionQueue.TryDequeue(out var action))
                    action.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to run action on MainThread with Exception {e} | {e.Message}");
            }
        }

        private static void Updated_Handler()
        {
            ProcessQueue();
        }

        private static void UpdateOnDestroyed_Handler()
        {
            CleanUp();
        }

        public static Coroutine StartCoroutine(Func<IEnumerator> broadcastPresence)
        {
            return _update.StartCoroutine(broadcastPresence());
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            _update.StopCoroutine(coroutine);
        }
    }
}