using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hammock
{
#if PORTABLE
    public abstract class HashAlgorithm
    {
        public abstract byte[] ComputeHash(byte[] data);
        public abstract byte[] Key { get; set; }
        public static HashAlgorithm HMACSHA1Instance { get; set; }
    }
#endif
}
