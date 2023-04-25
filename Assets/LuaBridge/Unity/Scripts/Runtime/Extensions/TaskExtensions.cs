using System.Collections;
using System.Threading.Tasks;

namespace LuaBridge.Core.Extensions
{
    public static class TaskExtensions
    {
        /*
        public static IEnumerator ToCoroutine(this Task task)
        {
            if (task == null)
                yield break;
            while (task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.WaitingForActivation ||
                   !task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
                throw task.Exception;
        }
        */

    }
}