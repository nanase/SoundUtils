/* SoundUtils

LICENSE - The MIT License (MIT)

Copyright (c) 2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;

namespace SoundUtils
{
    public class FilterBuffer<T>
    {
        private readonly int length;
        private readonly T[] data;
        private readonly Action<T[]> action;
        private int index;

        public T[] Data { get { return this.data; } }

        public FilterBuffer(int length, Action<T[]> action)
        {
            this.length = length;
            this.data = new T[length];
            this.action = action;
        }

        public void Push(T[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int input_index = 0;

            while (input_index < input.Length)
            {
                int copy_length = Math.Min(this.length - this.index, input.Length - input_index);

                Array.Copy(input, input_index, this.data, this.index, copy_length);

                this.index += copy_length;
                input_index += copy_length;

                if (this.index + 1 >= this.length)
                {
                    this.action(this.data);
                    this.index = 0;
                }
            }
        }

        public void Close()
        {
            if (this.index > 0)
            {
                Array.Clear(this.data, this.index, this.length - this.index);
                this.action(this.data);

                this.index = 0;
            }
        }
    }
}
