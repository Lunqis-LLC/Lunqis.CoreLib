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
using System.Threading.Tasks;

namespace Azumo.Extensions
{
    /// <summary>  
    /// 异步自动释放列表  
    /// </summary>  
    /// <typeparam name="T"></typeparam>  
    public class ListAsyncDisposable<T> : List<T>, IAsyncDisposable where T : IAsyncDisposable
    {
        /// <summary>  
        /// 异步释放资源  
        /// </summary>  
        /// <returns></returns>  
        public async ValueTask DisposeAsync()
        {
            foreach (var item in this)
                await item.DisposeAsync();

            // Suppress finalization to adhere to CA1816  
            GC.SuppressFinalize(this);
        }
    }
}
