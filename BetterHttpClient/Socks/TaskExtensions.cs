using System;
using System.Threading.Tasks;

namespace BetterHttpClient.Socks
{
    public static class TaskExtensions
    {
        public static IAsyncResult AsApm<T>(this Task<T>  task,
                                            AsyncCallback callback,
                                            object        state)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var tcs = new TaskCompletionSource<T>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    if (t.Exception != null) tcs.TrySetException(t.Exception.InnerExceptions);
                }
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                callback?.Invoke(tcs.Task);
            }, TaskScheduler.Default);
            return tcs.Task;
        }
    }
}