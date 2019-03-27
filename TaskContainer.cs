using SVN.Debug;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SVN.Tasks
{
    public static class TaskContainer
    {
        private static CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        public static Action<Exception> ExceptionHandler { get; set; }

        public static Task Run(Action action)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    if (TaskContainer.ExceptionHandler != null)
                    {
                        TaskContainer.ExceptionHandler(e);
                    }
                    else
                    {
                        Logger.Write(e.GetType().Name, e.Message, e.StackTrace);
                    }
                }
            }, TaskContainer.CancellationTokenSource.Token);
        }

        public static void Abort()
        {
            TaskContainer.CancellationTokenSource.Cancel();
        }
    }
}