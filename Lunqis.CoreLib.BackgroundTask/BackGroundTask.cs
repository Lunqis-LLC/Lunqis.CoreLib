//MIT License

//Copyright (c) 2022-2025 Azumo Lab

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
using System.Threading;
using System.Threading.Tasks;

namespace Lunqis.CoreLib.BackgroundTask
{
    /// <summary>
    /// 后台任务
    /// </summary>
    /// <remarks>
    /// <see cref="ITask"/> 接口的实现类，用于在后台执行任务
    /// </remarks>
    public abstract class BackGroundTask : ITask
    {
        /// <summary>
        /// 在后台任务开始之前执行的任务
        /// </summary>
        public List<IStartTask> StartTasks { get; } = new List<IStartTask>();

        /// <summary>
        /// 发生错误的重试次数
        /// </summary>
        protected int ErrorCount { get; set; } = 3;

        /// <summary>
        /// 发生错误后的延迟时间
        /// </summary>
        protected TimeSpan ErrorDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 执行接口的任务
        /// </summary>
        /// <remarks>
        /// 向线程池中添加一个异步任务
        /// </remarks>
        /// <param name="input">传入参数</param>
        /// <param name="token">token</param>
        /// <returns>异步执行</returns>
        public virtual Task ExecuteAsync(object? input, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
                foreach (var task in StartTasks)
                    return task.ExecuteAsync(input, token);

            if (!token.IsCancellationRequested)
                _ = ThreadPool.QueueUserWorkItem(async (inputObj) =>
                {
                    var errorCount = 0;
                ReExecute:
                    try
                    {
                        // 开始执行后台任务
                        if (!token.IsCancellationRequested)
                            await BackGroundExecuteAsync(inputObj, token);
                    }
                    catch (Exception) when (token.IsCancellationRequested)
                    {
                        return; // 立刻结束
                    }
                    catch (Exception)
                    {
                        errorCount++;
                        if (errorCount < ErrorCount)
                            try
                            {
                                await Task.Delay(ErrorDelay, token); // 重试
                            }
                            catch (TaskCanceledException)
                            {
                                return; // 立刻结束
                            }
                        else
                            return; // 重试后放弃
                        goto ReExecute;
                    }
                }, input);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 开始执行后台任务
        /// </summary>
        /// <param name="input">传入参数</param>
        /// <param name="token">Token <see cref="CancellationToken"/></param>
        /// <returns>异步任务</returns>
        protected abstract Task BackGroundExecuteAsync(object? input, CancellationToken token);
    }
}
