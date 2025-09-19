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
using System.IO;

namespace Lunqis.CoreLib.Errors
{
    public static class ErrorChecker
    {
        public static void IfNullThrow<T>(T obj, string paramName) where T : class
        {
            if (obj is null)
            {
                throw Error.ArgumentNull(paramName);
            }
        }

        public static void IfNullOrEmptyThrow(string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw Error.Argument(paramName + " cannot be null or empty.", paramName);
            }
        }

        public static void IfNullOrWhitespaceThrow(string str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw Error.Argument(paramName + " cannot be null or whitespace.", paramName);
            }
        }

        public static void IfFileNotFoundThrow(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw Error.FileNotFound($"File not found: {filePath}");
            }
        }
    }
}
