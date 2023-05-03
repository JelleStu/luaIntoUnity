using System;
using System.Collections;
using System.Threading.Tasks;

namespace LuaBridge.Unity.Tests.Tests.UIServiceTest.Extensions
{
    public static class TaskExtensions
    {
        public static IEnumerator ToCoroutine(this Task task)
        {
            while (!task.IsCompleted || task.Status == TaskStatus.WaitingForActivation || task.Exception != null)
                yield return null;
        }

        public static IEnumerator ToCoroutine<T>(this Task<T> task, Action<T> callback)
        {
            while (!task.IsCompleted || task.Status == TaskStatus.WaitingForActivation|| task.Exception != null)
                yield return null;
            callback?.Invoke(task.Result);
        }
    }
}