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
using Azumo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lunqis.CoreLib.Extensions
{
    public static partial class AzumoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Func<object, object?[]?, Task<object?>> BuildTaskFunc(this MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo), "MethodInfo cannot be null.");

            // 创建对象创建工厂
            var instanceType = methodInfo.DeclaringType;
            var isStatic = methodInfo.IsStatic;

            if (!isStatic && instanceType == null)
                throw new InvalidOperationException("Cannot create factory for static method with null declaring type.");

            // 参数
            var instance = Expression.Parameter(typeof(object), "instance");
            var paramArrays = Expression.Parameter(typeof(object[]), "param");
            var itemParamList = methodInfo.GetParameters().ConvertToExpression(paramArrays);

            // 实例类型转换
            Expression? instanceConvert = null;
            if (instanceType != null && !isStatic)
                instanceConvert = Expression.Convert(instance, instanceType);

            // 调用方法
            RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);

            // 呼叫函数
            var methodResultObject = Expression.Call(instanceConvert, methodInfo, itemParamList);

            // 返回值处理
            var returnResult = ReturnTypeProcess(methodInfo, methodResultObject);

            // 创建函数
            var result = Expression.Lambda<Func<object, object?[]?, Task<object?>>>(returnResult, instance, paramArrays).Compile();
            RuntimeHelpers.PrepareDelegate(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task ConvertTo(Task task) =>
            await task;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public static Expression ReturnTypeProcess(this MethodInfo methodInfo, Expression methodResult)
        {
            Expression returnExpression;
            var returnType = methodInfo.ReturnType;
            if (returnType == typeof(void))
                returnExpression = Expression.Constant(null, typeof(object));
            else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                // Task<T> 需要转换为 Task<object>，而类型则有可能是以下的任何类型
                // Task<String>, Task<int>, Task<Task<String>>, Task<Task<Task<int>>>

                // 先获取内部的泛型类型
                var childType = returnType.GetGenericArguments();
                // 判断内部的泛型类型是否是Task<T>
                while (childType[0].IsGenericType && childType[0].GetGenericTypeDefinition() == typeof(Task<>))
                {
                    childType = childType[0].GetGenericArguments();
                }

                var TaskObjResult = new List<Expression>();
                static Expression CreateExpression(Type type)
                {
                    // Task<Task<string>>
                    // 把 Task 类型 转换为 Task<Task<string>>，并调用Result
                    var TaskAwait = Expression.Parameter(typeof(Task), "TaskAwait");
                    var TaskConvert = Expression.Convert(TaskAwait, type);
                    // Task<string>
                    var result = Expression.Property(TaskConvert, nameof(Task<object>.Result));
                    Expression resuFunc = Expression.Lambda<Func<Task, object?>>(result, TaskAwait);
                    return resuFunc;
                }

                var convertTo = typeof(AzumoExtensions).GetMethod(nameof(ConvertTo), BindingFlags.Static | BindingFlags.NonPublic)!;

                // 呼叫ConvertTo方法
                var taskMethodResult = Expression.Call(null, convertTo, methodResult);
                Expression convertValue = Expression.Invoke(CreateExpression(returnType), methodResult);
                // 呼叫ConvertTo方法
                var callConvertTo123 = Expression.Block(taskMethodResult, convertValue);

                TaskObjResult.Add(callConvertTo123);

                //var childType = returnType.GetGenericArguments();
                var expression = convertValue;

                while (childType[0].IsGenericType && childType[0].GetGenericTypeDefinition() == typeof(Task<>))
                {
                    // 将Object类型转换为 内部的 泛型
                    var taskValue = Expression.Convert(expression, childType[0]);
                    // 呼叫ConvertTo方法，并将新参数传入
                    var callConvertTo = Expression.Call(null, convertTo, taskValue);
                    // 返回新的泛型类型
                    expression = Expression.Invoke(CreateExpression(childType[0]), taskValue);
                    var awaitTaskObj = Expression.Block(callConvertTo, taskValue);

                    TaskObjResult.Add(awaitTaskObj);
                    childType = childType[0].GetGenericArguments();
                }

                TaskObjResult.Add(expression);

                var methodReturnInstance = Expression.Parameter(returnType);
                returnExpression = Expression.Invoke(Expression.Lambda(Expression.Block(TaskObjResult), methodReturnInstance), methodResult);
            }
            else
                returnExpression = Expression.Convert(methodResult, typeof(object));
            return returnExpression;
        }
    }
}
