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
using Lunqis.CoreLib.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lunqis.CoreLib.Extensions
{
    public static partial class AzumoExtensions
    {
        #region 将字符串类型转换为其他类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileInfo? AsFileInfo(this string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            var fileInfo = new FileInfo(filePath);
            return fileInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static DirectoryInfo? AsDirectoryInfo(this string? directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                return null;
            var directoryInfo = new DirectoryInfo(directoryPath);
            return directoryInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static int AsInt(this string? str, int defualt = 0) =>
            string.IsNullOrEmpty(str) ? defualt : int.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static long AsLong(this string? str, long defualt = 0) =>
            string.IsNullOrEmpty(str) ? defualt : long.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static double AsDouble(this string? str, double defualt = 0) =>
            string.IsNullOrEmpty(str) ? defualt : double.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static decimal AsDecimal(this string? str, decimal defualt = 0) =>
            string.IsNullOrEmpty(str) ? defualt : decimal.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static bool AsBool(this string? str, bool defualt = false) =>
            string.IsNullOrEmpty(str) ? defualt : bool.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this string? str, DateTime defualt) =>
            string.IsNullOrEmpty(str) ? defualt : DateTime.TryParse(str, out var result) ? result : defualt;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defualt"></param>
        /// <returns></returns>
        public static float AsFloat(this string? str, float defualt = 0) =>
            string.IsNullOrEmpty(str) ? defualt : float.TryParse(str, out var result) ? result : defualt;
        #endregion

        #region 将字符串当成文件路径的一部分进行处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFileName(this string? str) =>
            Path.GetFileName(str ?? string.Empty);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(this string? str) =>
            Path.GetFileNameWithoutExtension(str ?? string.Empty);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newExtension"></param>
        /// <returns></returns>
        public static string ChangeExtension(this string? str, string? newExtension) =>
            Path.ChangeExtension(str ?? string.Empty, newExtension);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newExtension"></param>
        /// <returns></returns>
        public static string AppendExtension(this string? str, string? newExtension) =>
            str + newExtension;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Stream OpenFileAsStream(this string str) =>
            File.Open(str, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StreamReader OpenFileAsStreamReader(this string str) =>
            new StreamReader(str.OpenFileAsStream());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static StreamReader OpenFileAsStreamReader(this string str, Encoding encoding) =>
            new StreamReader(str.OpenFileAsStream(), encoding);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StreamWriter OpenFileAsStreamWriter(this string str) =>
            new StreamWriter(str.OpenFileAsStream());
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static StreamWriter OpenFileAsStreamWriter(this string str, Encoding encoding) =>
            new StreamWriter(str.OpenFileAsStream(), encoding);
        #endregion

        #region 字符串操作处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StringBuilder GetStringBuilder(this string str) =>
            new StringBuilder(str);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] Split(this string str, char separator, int minCount, int maxCount, StringSplitOptions stringSplitOptions)
        {
            var result = str.Split(separator, maxCount, stringSplitOptions);
            if (result.Length < minCount)
            {
                var newResult = new string[minCount];
                Array.Copy(result, newResult, result.Length);
                for (var i = result.Length; i < minCount; i++)
                    newResult[i] = string.Empty;
                result = newResult;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PasswordGenerator(this string str, int length = 8)
        {
            var chars = str.Trim();
            var password = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < length; i++)
                _ = password.Append(chars[random.Next(chars.Length)]);
            return password.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSha256(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion
    }
}
