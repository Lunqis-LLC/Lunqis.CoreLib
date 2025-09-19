//MIT License

//Copyright (c) 2025-2025 Lunqis LLC

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lunqis.CoreLib.BackgroundTask
{
    internal static class StaticTimer
    {
        public static CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();
        static StaticTimer()
        {
            _ = ThreadPool.QueueUserWorkItem(async (obj) => await Execute((CancellationTokenSource)obj), TokenSource);
        }
        private static async Task Execute(CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.1));
                foreach (TaskWrapper task in tasks)
                {
                    if (!task.Enabled)
                    {
                        continue;
                    }
                    if (task.NextRunTime <= DateTime.Now)
                    {
                        _ = ThreadPool.QueueUserWorkItem(async (input) =>
                        {
                            int reTryCount = 3;
                        RE_TRY:
                            CancellationTokenSource cancellationToken = (CancellationTokenSource)input;
                            try
                            {
                                await task.Task.ExecuteAsync(null, cancellationToken.Token);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                            catch (Exception)
                            {
                                reTryCount--;
                                if (reTryCount > 0 && !cancellationToken.IsCancellationRequested)
                                {
                                    goto RE_TRY;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }, TokenSource);
                    }
                }
            }
        }
        public static void Stop()
        {
            TokenSource.Cancel();
        }
        public static void Init(CancellationTokenSource cancellationTokenSource)
        {
            if (TokenSource.IsCancellationRequested)
            {
                throw new InvalidOperationException("StaticTimer has been stopped and cannot be restarted.");
            }

            _ = cancellationTokenSource.Token.Register(() => TokenSource.Cancel());
        }
        public static void RegisterTask(TaskWrapper taskWrapper)
        {
            tasks.Add(taskWrapper);
        }
        public static void UnregisterTask(TaskWrapper taskWrapper)
        {
            tasks.Remove(taskWrapper);
        }
        private static readonly List<TaskWrapper> tasks = new List<TaskWrapper>();
    }

    internal class TaskWrapper
    {
        public ITask Task { get; }
        public DateTime NextRunTime
        {
            get
            {
                if (tasksRunTimes.Count == 0)
                    tasksRunTimes.AddRange(Runtimes);

                if (tasksRunTimes.Count == 0)
                    throw new InvalidOperationException("No run times available.");

                DateTime dateTime = tasksRunTimes.FirstOrDefault();
                if (dateTime <= DateTime.Now)
                {
                    tasksRunTimes.RemoveAt(0);
                    if (Interval != TimeSpan.Zero)
                        tasksRunTimes.Add(dateTime.Add(Interval));
                    else
                        tasksRunTimes.Add(dateTime.AddDays(1));
                }
                
                return dateTime;
            }
            set
            {
                Runtimes.Clear();
                Runtimes.Add(value);
            }
        }
        private readonly List<DateTime> tasksRunTimes = new List<DateTime>();
        private readonly List<DateTime> Runtimes = new List<DateTime>();
        public TimeSpan Interval { get; }
        public bool Enabled { get; set; } = true;
        public TaskWrapper(ITask task, TimeSpan interval)
        {
            Task = task;
            Interval = interval;
            NextRunTime = DateTime.Now.Add(interval);
        }
        public TaskWrapper(ITask task, TimeSpan interval, DateTime firstRunTime)
        {
            Task = task;
            Interval = interval;
            NextRunTime = firstRunTime > DateTime.Now ? firstRunTime : DateTime.Now.Add(interval);
        }
        public TaskWrapper(ITask task, DateTime[] runtimes)
        {
            if (runtimes == null || runtimes.Length == 0)
            {
                throw new ArgumentException("Runtimes cannot be null or empty.", nameof(runtimes));
            }
            Task = task;
            Runtimes.AddRange(runtimes);
            Interval = TimeSpan.Zero;
        }
    }
}
