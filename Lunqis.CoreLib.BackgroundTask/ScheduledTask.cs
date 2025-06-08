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
    /// <summary>
    /// 按计划执行的任务
    /// </summary>
    public abstract class ScheduledTask : BackGroundTask
    {
        /// <summary>
        /// 执行任务时间表
        /// </summary>
        private readonly List<DateTime?> InvokeTimes = new List<DateTime?>();

        /// <summary>
        /// 一组计划之间的间隔
        /// </summary>
        /// <remarks>
        /// 例如，这个值是一天的情况下，任务将会每天执行一次。
        /// </remarks>
        protected TimeSpan IntervalTimeSpan { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// 开始执行异步任务
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <param name="token">Token</param>
        /// <returns>异步任务</returns>
        protected override async Task BackGroundExecuteAsync(object? input, CancellationToken token)
        {
            var orderTimes = InvokeTimes.OrderBy(x => x).ToList();
            InvokeTimes.Clear();
            InvokeTimes.AddRange(orderTimes);

            while (!token.IsCancellationRequested)
            {
                var time = InvokeTimes.FirstOrDefault();
                if (time == null)
                    return;

                if (DateTime.Now >= time)
                {
                    // 执行任务
                    await ScheduledExecuteAsync(input, token);
                    _ = InvokeTimes.Remove(time);
                    var newTime = time.Value.Add(IntervalTimeSpan);

                    // 添加新的执行时间
                    if (newTime > DateTime.Now)
                        InvokeTimes.Add(newTime);
                }

                await Task.Delay(TimeSpan.FromSeconds(60), token);
            }
        }

        /// <summary>
        /// 任务计划表执行的任务
        /// </summary>
        /// <param name="input">参数</param>
        /// <param name="token">Token</param>
        /// <returns></returns>
        protected abstract Task ScheduledExecuteAsync(object? input, CancellationToken token);

        /// <summary>
        /// 添加一个计划时间
        /// </summary>
        /// <param name="hour">小时</param>
        /// <param name="minute">分钟</param>
        /// <param name="second">秒</param>
        protected void AddScheduled(int hour, int minute = 0, int second = 0) =>
            InvokeTimes.Add(DateTime.Now.Date.AddHours(hour).AddMinutes(minute).AddSeconds(second));
    }
}
