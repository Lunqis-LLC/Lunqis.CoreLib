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
using System.Threading;
using System.Threading.Tasks;

namespace Lunqis.CoreLib.BackgroundTask
{
    /// <summary>
    /// 按定时间隔执行的任务
    /// </summary>
    public abstract class TimerTask : BackGroundTask
    {
        /// <summary>
        /// 任务执行的时间
        /// </summary>
        private DateTime ExecuteDatetime { get; set; }

        /// <summary>
        /// 任务执行的间隔
        /// </summary>
        protected TimeSpan Interval { get; set; }

        /// <summary>
        /// 开始执行后台任务
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <param name="token">Token</param>
        /// <returns>异步任务</returns>
        protected override async Task BackGroundExecuteAsync(object? input, CancellationToken token)
        {
            // 如果任务没有取消，则始终执行
            while (!token.IsCancellationRequested)
                // 如果当前时间大于等于执行时间，则执行任务
                if (DateTime.Now >= ExecuteDatetime)
                {
                    // 计算下一次执行的时间
                    ExecuteDatetime = DateTime.Now.Add(Interval);
                    await IntervalExecuteAsync(input, token);
                }
                else
                    await Task.Delay(Interval, token);
        }

        /// <summary>
        /// 执行定期间隔的任务
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <param name="token">Token</param>
        /// <returns>异步任务</returns>
        protected abstract Task IntervalExecuteAsync(object? input, CancellationToken token);
    }
}
