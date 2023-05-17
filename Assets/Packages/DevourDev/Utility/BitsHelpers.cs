namespace DevourDev.Utility
{
    public static class BitsHelpers
    {
        public static byte Add(byte a, byte b)
        {
            return (byte)(Add(a, (int)b));
        }

        public static ushort Add(ushort a, ushort b)
        {
            return (ushort)Add(a, (int)b);
        }

        public static long Add(long a, long b)
        {
            return a | b;
        }

        public static ulong Add(ulong a, ulong b)
        {
            return a | b;
        }

        public static uint Add(uint a, uint b)
        {
            return a | b;
        }

        public static int Add(int a, int b)
        {
            return a | b;
        }


        public static byte Remove(byte from, byte toRemove)
        {
            return Remove(from, toRemove);
        }

        public static ushort Remove(ushort from, ushort toRemove)
        {
            return Remove(from, toRemove);
        }

        public static long Remove(long from, long toRemove)
        {
            return from & ~toRemove;
        }

        public static ulong Remove(ulong from, ulong toRemove)
        {
            return from & ~toRemove;
        }

        public static int Remove(int from, int toRemove)
        {
            return from & ~toRemove;
        }

        public static uint Remove(uint from, uint toRemove)
        {
            return from & ~toRemove;
        }


        public static bool Contains(byte bits, byte value)
        {
            return Contains(bits, value);
        }

        public static bool Contains(ushort bits, ushort value)
        {
            return Contains(bits, value);
        }

        public static bool Contains(int bits, int value)
        {
            return (bits & value) != 0;
        }

        public static bool Contains(uint bits, uint value)
        {
            return (bits & value) != 0;
        }

        public static bool Contains(long bits, long value)
        {
            return (bits & value) != 0;
        }

        public static bool Contains(ulong bits, ulong value)
        {
            return (bits & value) != 0;
        }
    }
}
