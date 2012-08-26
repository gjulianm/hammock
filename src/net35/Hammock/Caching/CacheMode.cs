using System;
using System.Runtime.Serialization;

namespace Hammock.Caching
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public enum CacheMode
    {
#if !SILVERLIGHT && !PORTABLE && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch && !NETCF
        [EnumMember] NoExpiration,
        [EnumMember] AbsoluteExpiration,
        [EnumMember] SlidingExpiration
#else
      NoExpiration,
        AbsoluteExpiration,
        SlidingExpiration
#endif
    }
}