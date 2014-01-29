using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundUtils
{
    public unsafe static class BitOperate
    {
        #region -- Public Static Methods --
        public static long ReverseBytes(long value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1, b2 = b0 + 2, b3 = b0 + 3,
                b4 = b0 + 4, b5 = b0 + 5, b6 = b0 + 6, b7 = b0 + 7;
                return *b0 * 72057594037927936 + *b1 * 281474976710656 +
                    *b2 * 1099511627776 + *b3 * 4294967296 +
                    *b4 * 16777216 + *b5 * 65536 +
                    *b6 * 256 + *b7;
            }
            else
                return value;
        }

        public static int ReverseBytes(int value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1, b2 = b0 + 2, b3 = b0 + 3;
                return *b0 * 16777216 + *b1 * 65536 + *b2 * 256 + *b3;
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
