using System;

namespace Hammock.Silverlight.Compat
{
#if !METRO
    [Flags]
    public enum DecompressionMethods
    {
        Deflate = 2,
        GZip = 4,
        None = 6
    }
#endif
}
