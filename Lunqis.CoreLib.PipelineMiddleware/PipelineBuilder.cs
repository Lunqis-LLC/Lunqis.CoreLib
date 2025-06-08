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

namespace Lunqis.CoreLib.PipelineMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal class PipelineBuilder<TInput, TResult> : IPipelineBuilder<TInput, TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defVal"></param>
        public PipelineBuilder(Func<TResult> defVal) =>
            this.defVal = defVal;

        /// <summary>
        /// 
        /// </summary>
        private readonly Func<TResult> defVal;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<object, List<IMiddleware<TInput, TResult>>> pipelines =
#if NET8_0_OR_GREATER
            [];
#else
            new Dictionary<object, List<IMiddleware<TInput, TResult>>>();
#endif

        /// <summary>
        /// 
        /// </summary>
        private readonly List<IMiddleware<TInput, TResult>> middleware =
#if NET8_0_OR_GREATER
            [];
#else
            new List<IMiddleware<TInput, TResult>>();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPipelineController<TInput, TResult> Build()
        {
            var pipelineDic = new Dictionary<object, IPipeline<TInput, TResult>>();
            if (pipelines.Count == 0)
                _ = CreatePipeline(Guid.NewGuid().ToString());
            foreach (var item in pipelines)
            {
                PipelineMiddlewareDelegate<TInput, TResult> handleResult = input => defVal();
                var list = new List<Func<PipelineMiddlewareDelegate<TInput, TResult>, PipelineMiddlewareDelegate<TInput, TResult>>>();
                foreach (var middleware in item.Value)
                    list.Add(handle => input => middleware.Execute(input, handle));
                foreach (var handle in list.Reverse<Func<PipelineMiddlewareDelegate<TInput, TResult>, PipelineMiddlewareDelegate<TInput, TResult>>>())
                    handleResult = handle(handleResult);

                pipelineDic.Add(item.Key, PipelineFactory.GetPipeline(handleResult));
            }
            return PipelineFactory.GetPipelineController(pipelineDic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IPipelineBuilder<TInput, TResult> CreatePipeline(object key)
        {
            if (middleware.Count == 0)
                throw new Exception();
            pipelines.Add(key, new List<IMiddleware<TInput, TResult>>(middleware));
            middleware.Clear();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        public IPipelineBuilder<TInput, TResult> Use(IMiddleware<TInput, TResult> middleware)
        {
            this.middleware.Add(middleware);
            return this;
        }
    }
}
