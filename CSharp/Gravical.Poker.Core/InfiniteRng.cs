using System;
using System.Security.Cryptography;

namespace Gravical.Poker.Core
{
    public static class InfiniteRng
    {
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();
        private static readonly Random Rnd = new Random();

        public static byte[] GetBytes(int count)
        {
            var random = new byte[count];
            Rng.GetBytes(random);
            return random;
        }

        public static int Next()
        {
            return GetPositiveInt32Array(1)[0];
        }

        public static int[] GetPositiveInt32Array(int count)
        {
            var ints = new int[count];
            for (int i = 0; i < count; i++)
            {
                ints[i] = Rnd.Next();
            }
            return ints;
        }
    }
}