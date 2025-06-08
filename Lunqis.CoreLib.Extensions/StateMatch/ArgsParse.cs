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
using System.Collections.Generic;
using System.Text;

namespace Lunqis.CoreLib.Extensions.StateMatch
{
    /// <summary>
    /// <para>ArgsParse 类用于解析命令行参数字符串，并将其转换为键值对的形式。</para>
    /// <para>该类实现了一个简单的状态机，用于处理参数的分隔符、引号包裹的值等复杂情况。</para>
    /// <para>这是一个单例类，使用 <see cref="Instance"/> 属性获取唯一实例。</para>
    /// </summary>
    public class ArgsParse
    {
        /// <summary>
        /// <para>获取 ArgsParse 类的唯一实例。</para>
        /// <para>这是一个单例模式的实现，确保整个应用程序中只有一个 ArgsParse 实例。</para>
        /// </summary>
        public static ArgsParse Instance { get; } = new ArgsParse();

        /// <summary>
        /// <para>State 类表示状态机中的一个状态。</para>
        /// <para>每个状态可以有多个字符到下一个状态的转换规则，以及一个通配符转换规则。</para>
        /// </summary>
        private class State
        {
            /// <summary>
            /// <para>指示当前状态是否为终止状态。</para>
            /// <para>如果为 true，则表示当前状态是一个有效的结束状态。</para>
            /// </summary>
            public bool IsFinal { get; set; }

            /// <summary>
            /// <para>存储从当前状态到其他状态的字符转换规则。</para>
            /// <para>键为触发转换的字符，值为目标状态。</para>
            /// </summary>
            public Dictionary<char, State> Transitions { get; set; } = new Dictionary<char, State>();

            /// <summary>
            /// <para>通配符转换规则。</para>
            /// <para>如果当前输入字符没有匹配的转换规则，则使用此规则进行状态转换。</para>
            /// </summary>
            public State? WildcardTransition { get; set; }
        }

        /// <summary>
        /// 当前状态机的状态。
        /// </summary>
        private State currentState;

        /// <summary>
        /// 状态机的起始状态。
        /// </summary>
        private readonly State startState;

        /// <summary>
        /// <para>私有构造函数，用于初始化状态机。</para>
        /// <para>该构造函数定义了状态机的所有状态和转换规则。</para>
        /// </summary>
        private ArgsParse()
        {
            startState = new State();

            var nextState = new State();
            startState.Transitions.Add(' ', nextState);
            nextState.IsFinal = true;

            var blockState = new State();
            startState.Transitions.Add('"', blockState);
            blockState.WildcardTransition = blockState;
            var endState = new State();
            blockState.Transitions.Add('"', endState);
            endState.IsFinal = true;

            currentState = startState;
        }

        /// <summary>
        /// <para>解析命令行参数字符串，并将其转换为键值对的形式。</para>
        /// <para>支持以下功能：</para>
        /// <list type="bullet">
        /// <item>以空格分隔的参数。</item>
        /// <item>使用双引号包裹的参数值。</item>
        /// </list>
        /// </summary>
        /// <remarks>
        /// <para>注意事项：</para>
        /// <list type="bullet">
        /// <item>参数名和值之间必须用空格分隔。</item>
        /// <item>如果参数值包含空格，必须用双引号包裹。</item>
        /// </list>
        /// <example>
        /// <code>
        /// var parser = ArgsParse.Instance;
        /// var result = parser.StartArgsParse("key1 value1 \"key2 value2\"");
        /// // result: { ["key1"] = "value1", ["key2"] = "value2" }
        /// </code>
        /// </example>
        /// </remarks>
        /// <param name="args">要解析的命令行参数字符串。</param>
        /// <returns>
        /// <para>返回一个字典，其中键为参数名，值为参数值。</para>
        /// <para>如果参数没有值，则值为空字符串。</para>
        /// </returns>
        public Dictionary<string, string> StartArgsParse(string args)
        {
            args += " ";
            var value = new StringBuilder();
            var result = new Dictionary<string, string>();

            var key = string.Empty;
            foreach (var arg in args)
            {
                if (currentState.Transitions.TryGetValue(arg, out var nextState))
                {
                    currentState = nextState;
                    if (currentState.IsFinal)
                    {
                        currentState = startState;

                        var valueString = value.ToString();
                        if (string.IsNullOrEmpty(valueString))
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(key))
                        {
                            key = valueString;
                        }
                        else
                        {
                            var val = valueString;
                            result.Add(key, val);
                            key = string.Empty;
                            val = string.Empty;
                        }
                        _ = value.Clear();
                    }
                }
                else if (currentState.WildcardTransition != null)
                {
                    currentState = currentState.WildcardTransition;
                    _ = value.Append(arg);
                }
                else
                {
                    _ = value.Append(arg);
                }
            }
            return result;
        }
    }
}
