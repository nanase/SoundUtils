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
