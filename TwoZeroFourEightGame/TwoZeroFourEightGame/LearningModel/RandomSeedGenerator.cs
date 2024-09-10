using System;

namespace RandomSeedGenerator
{
    class RSG
    {
        public static int CreateSeedFromNewGuid()
        {
            return Guid.NewGuid().GetHashCode();
        }

        public static int CreateSeedFromGuid(Guid guid)
        {
            return guid.GetHashCode();
        }

        public static Guid CreateNewGuid()
        {
            return Guid.NewGuid();
        }

        public static Guid CreateCryptographicallyStrongGuid()
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var data = new byte[16];
            rng.GetBytes(data);
            return new Guid(data);
        }
    }
}