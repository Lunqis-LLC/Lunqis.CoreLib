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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunqis.CoreLib.Extensions.StateMatch
{
    public class BlockParse : IBlockParser
    {
        private class State
        {
            public bool IsFinal { get; set; }
            public Dictionary<char, State> Transitions { get; set; } = new Dictionary<char, State>();
            public State? WildcardTransition { get; set; }
        }

        private readonly State startState = new State();
        private State currentState;

        public BlockParse()
        {
            currentState = startState;
            Value = string.Empty;
        }

        public string Value { get; set; }

        public IBlockParser EndWith(string end)
        {
            foreach (var c in end)
            {
                currentState.WildcardTransition = currentState;

                var nextState = new State();
                if (currentState.Transitions.TryGetValue(c, out var existingState))
                {
                    currentState = existingState;
                    continue;
                }
                currentState.Transitions[c] = nextState;
                currentState = nextState;
            }
            currentState.IsFinal = true;
            return this;
        }

        public IBlockParser StartWith(string start)
        {
            foreach (var c in start)
            {
                var nextState = new State();
                if (currentState.Transitions.TryGetValue(c, out var existingState))
                {
                    currentState = existingState;
                    continue;
                }
                currentState.Transitions[c] = nextState;
                currentState = nextState;
            }
            return this;
        }

        public bool Find(string text)
        {
            currentState = startState;
            var contextValue = new StringBuilder();
            foreach (var c in text)
            {
                if (currentState.IsFinal)
                    break;

                if (currentState.Transitions.TryGetValue(c, out var result))
                    currentState = result;
                else if (currentState.WildcardTransition != null)
                    contextValue.Append(c);
                else
                    currentState = startState;
            }
            if (currentState.IsFinal)
                Value = contextValue.ToString();
            return currentState.IsFinal;
        }
    }
}
