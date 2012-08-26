using System;
using System.Runtime.Serialization;

namespace Hammock.Tasks
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public enum RateLimitType
    {
#if !SILVERLIGHT && !PORTABLE && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch && !NETCF
        [EnumMember] ByPercent,
        [EnumMember] ByPredicate
#else
      ByPercent,
        ByPredicate
#endif
    }
}