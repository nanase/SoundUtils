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

namespace SoundUtils
{
    public unsafe static class BitOperate
    {
        #region -- Public Static Methods --
        public static long ReverseBytes(long value, bool reverse)
        {
            if (reverse)
            {
                byte* x = stackalloc byte[8];
                byte* bp = (byte*)&value;
                x[0] = bp[7];
                x[1] = bp[6];
                x[2] = bp[5];
                x[3] = bp[4];
                x[4] = bp[3];
                x[5] = bp[2];
                x[6] = bp[1];
                x[7] = bp[0];

                return *(long*)x;
            }
            else
                return value;
        }

        public static int ReverseBytes(int value, bool reverse)
        {
            if (reverse)
            {
                byte* x = stackalloc byte[4];
                byte* bp = (byte*)&value;
                x[0] = bp[3];
                x[1] = bp[2];
                x[2] = bp[1];
                x[3] = bp[0];

                return *(int*)x;
            }
            else
                return value;
        }

        public static short ReverseBytes(short value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1;
                return (short)(*b0 * 256 + *b1);
            }
            else
                return value;
        }
        #endregion
    }
}
