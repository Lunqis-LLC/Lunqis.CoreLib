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
using System.IO;

namespace Lunqis.CoreLib.Extensions
{
#if DEBUG
    public static class DebugLog
    {
        private const string DebugLogFile = "Azumo.DebugLog.log";
        private const string ErrorLogFile = "Azumo.ErrorLog.log";
        static DebugLog()
        {
            CreateFile(DebugLogFile);
            CreateFile(ErrorLogFile);
        }

        public static void Debug(string message) => Log(DebugLogFile, message);

        public static void Error(string message) => Log(ErrorLogFile, message);

        private static void Log(string file, string message)
        {
            System.Diagnostics.Debug.Print($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            using (var streamWriter = new StreamWriter(File.Open(file, FileMode.Append)))
            {
                streamWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }

        private static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
        }
    }
#endif
}
