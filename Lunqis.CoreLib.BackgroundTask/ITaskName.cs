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
namespace Lunqis.CoreLib.BackgroundTask
{
    /// <summary>
    /// <c>ITaskName</c> 接口定义了一个任务名称的属性，用于为实现该接口的任务类提供统一的任务名称标识。
    /// </summary>
    /// <remarks>
    /// 该接口的主要作用是为任务类提供一个标准化的方式来获取任务名称，
    /// 以便在任务调度、日志记录或其他需要标识任务的场景中使用。
    /// 
    /// <para>使用场景：</para>
    /// <list type="bullet">
    /// <item>任务调度系统中，用于通过任务名称查找和管理任务。</item>
    /// <item>日志记录中，用于标识任务的来源或类型。</item>
    /// <item>动态加载任务时，用于通过名称查找对应的任务实例。</item>
    /// </list>
    /// 
    /// <para>实现要求：</para>
    /// <list type="bullet">
    /// <item>实现该接口的类必须提供 <c>TaskName</c> 属性的具体实现。</item>
    /// <item>任务名称应具有唯一性，以避免冲突。</item>
    /// </list>
    /// 
    /// <para>示例代码：</para>
    /// <code>
    /// public class ExampleTask : ITaskName
    /// {
    ///     public string TaskName => "ExampleTask";
    /// }
    /// </code>
    /// </remarks>
    public interface ITaskName
    {
        /// <summary>
        /// 获取任务的名称。
        /// </summary>
        /// <value>任务的唯一名称。</value>
        public string TaskName { get; }
    }
}
