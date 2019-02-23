using SVN.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SVN.Tasks
{
    public static class TaskContainer
    {
        private static int Counter { get; set; }
        private static List<int> List { get; } = new List<int>();
        private static CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public static bool Any
        {
            get => TaskContainer.List.Any();
        }

        public static Task Run(Action action)
        {
            var id = ++TaskContainer.Counter;
            TaskContainer.List.Add(id);

            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Logger.Write(e.GetType().Name, e.Message, e.StackTrace);
                }
                TaskContainer.List.Remove(id);
            }, TaskContainer.CancellationTokenSource.Token);

            return task;
        }

        public static void Abort()
        {
            TaskContainer.CancellationTokenSource.Cancel();
        }
    }
}