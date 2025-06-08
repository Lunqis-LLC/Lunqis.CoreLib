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

namespace Lunqis.CoreLib.BackgroundTask
{
    /// <summary>
    /// <c>TaskNameAttribute</c> 是一个自定义特性，用于为任务类指定一个唯一的任务名称。
    /// </summary>
    /// <remarks>
    /// 该特性主要用于标记任务类，并为其分配一个唯一的任务名称，
    /// 以便在任务调度、日志记录或其他需要标识任务的场景中使用。
    /// 
    /// <para>使用场景：</para>
    /// <list type="bullet">
    /// <item>任务调度系统中，用于区分不同的任务。</item>
    /// <item>日志记录中，用于标识任务的来源或类型。</item>
    /// <item>动态加载任务时，用于通过名称查找对应的任务类。</item>
    /// </list>
    /// 
    /// <para>限制：</para>
    /// <list type="bullet">
    /// <item>该特性只能应用于类，不能应用于方法、属性等其他成员。</item>
    /// <item>每个类只能应用一次该特性，不能重复使用。</item>
    /// </list>
    /// 
    /// <para>示例代码：</para>
    /// <code>
    /// [TaskName("ExampleTask")]
    /// public class ExampleTask : ITask
    /// {
    ///     // 任务实现代码
    /// }
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TaskNameAttribute : Attribute
    {
        /// <summary>
        /// 获取任务的名称。
        /// </summary>
        /// <value>任务的唯一名称。</value>
        public string TaskName { get; }

        public TaskNameAttribute(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空或空白。", nameof(taskName));
            TaskName = taskName;
        }
    }
}
