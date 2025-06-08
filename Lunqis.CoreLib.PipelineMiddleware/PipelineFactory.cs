//MIT License

//Copyright (c) 2025-2025 Lunqis

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

namespace Lunqis.CoreLib.PipelineMiddleware
{
    /// <summary>
    /// 管道工厂
    /// </summary>
    public class PipelineFactory
    {
        /// <summary>
        /// 创建一个管道构建器
        /// </summary>
        /// <typeparam name="TInput">传入类型</typeparam>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <returns>管道构造器 <see cref="IPipelineBuilder{TInput, TResult}"/> 的实例</returns>
        public static IPipelineBuilder<TInput, TResult> GetPipelineBuilder<TInput, TResult>(Func<TResult> defVal) =>
            new PipelineBuilder<TInput, TResult>(defVal);

        /// <summary>
        /// 内部方法，用于创建一个管道
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="middleware"></param>
        /// <returns></returns>
        internal static IPipeline<TInput, TResult> GetPipeline<TInput, TResult>(PipelineMiddlewareDelegate<TInput, TResult> middleware) =>
            new Pipeline<TInput, TResult>(middleware);

        /// <summary>
        /// 内部方法，用于创建一个管道控制器
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="dics"></param>
        /// <returns></returns>
        internal static IPipelineController<TInput, TResult> GetPipelineController<TInput, TResult>(Dictionary<object, IPipeline<TInput, TResult>> dics) =>
            new PipelineController<TInput, TResult>(dics);
    }
}
