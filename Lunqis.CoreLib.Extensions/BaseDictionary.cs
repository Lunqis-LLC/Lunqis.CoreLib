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

namespace Lunqis.CoreLib.Extensions
{
    /// <summary>
    /// 一个通用的字典类，继承自 <see cref="Dictionary{K, V}"/>。
    /// 提供了一些额外的功能，例如添加或更新键值对、获取值以及带默认值的获取方法。
    /// </summary>
    /// <typeparam name="K">字典的键类型，必须是非空类型。</typeparam>
    /// <typeparam name="V">字典的值类型，必须是引用类型。</typeparam>
    internal class BaseDictionary<K, V> : Dictionary<K, V> where K : notnull where V : class
    {
        /// <summary>
        /// 添加或更新字典中的键值对。
        /// 如果键已存在，则更新其值；如果键不存在，则添加新的键值对。
        /// </summary>
        /// <param name="key">要添加或更新的键。</param>
        /// <param name="value">与键关联的值。</param>
        /// <remarks>
        /// 使用此方法可以避免手动检查键是否存在。
        /// </remarks>
        public void AddOrUpdate(K key, V value)
        {
            if (!TryAdd(key, value))
                this[key] = value;
        }

        /// <summary>
        /// 根据键获取值。
        /// 如果键存在，则返回对应的值；如果键不存在，则返回 <c>null</c>。
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <returns>与键关联的值，如果键不存在则返回 <c>null</c>。</returns>
        /// <remarks>
        /// 使用此方法时需要注意返回值可能为 <c>null</c>，调用方应进行空值检查。
        /// </remarks>
        public V? Get(K key)
        {
            _ = TryGet(key, out var value);
            return value;
        }

        /// <summary>
        /// 根据键获取值，如果键不存在则返回默认值。
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <param name="defVal">当键不存在时返回的默认值的生成函数。</param>
        /// <returns>与键关联的值，如果键不存在则返回默认值。</returns>
        /// <remarks>
        /// 默认值通过 <paramref name="defVal"/> 参数生成，因此可以是动态计算的值。
        /// </remarks>
        public V? GetOrDefault(K key, Func<V> defVal) =>
            TryGet(key, out var value) ? value : defVal();

        /// <summary>
        /// 尝试根据键获取值。
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <param name="v">如果键存在，则输出与键关联的值；如果键不存在，则输出 <c>null</c>。</param>
        /// <returns>如果键存在则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <remarks>
        /// 此方法类似于 <see cref="Dictionary{K, V}.TryGetValue"/>，但返回值类型为可空类型。
        /// </remarks>
        public bool TryGet(K key, out V? v) =>
            TryGetValue(key, out v);

        /// <summary>
        /// 尝试根据键获取值，如果键不存在则返回默认值。
        /// </summary>
        /// <param name="key">要查找的键。</param>
        /// <param name="v">如果键存在，则输出与键关联的值；如果键不存在，则输出默认值。</param>
        /// <param name="defVal">当键不存在时返回的默认值的生成函数。</param>
        /// <returns>如果键存在则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <remarks>
        /// 默认值通过 <paramref name="defVal"/> 参数生成，因此可以是动态计算的值。
        /// </remarks>
        public bool TryGet(K key, out V v, Func<V> defVal)
        {
            bool result;
            if (!(result = TryGet(key, out var outVal)))
                outVal = defVal();
            v = outVal!;
            return result;
        }
    }
}
