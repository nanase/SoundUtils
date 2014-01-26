﻿/* SoundUtils

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
    public class Channel
    {
        #region -- Public Static Methods --
        public static void Split<T>(T[] source, T[] lch, T[] rch)
        {
            for (int i = 0, j = 0; i < source.Length / 2; i++)
            {
                lch[i] = source[j++];
                rch[i] = source[j++];
            }
        }

        public static void Join<T>(T[] lch, T[] rch, T[] dest)
        {
            for (int i = 0; i < dest.Length; i++)
            {
                if ((i & 1) == 0)
                    dest[i] = lch[i / 2];
                else
                    dest[i] = rch[i / 2];
            }
        }
        #endregion
    }
}
