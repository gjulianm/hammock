using System;
using System.Runtime.Serialization;

namespace Hammock.Web
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public enum HttpPostParameterType
    {
#if !SILVERLIGHT && !PORTABLE && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch && !NETCF
        [EnumMember] Field,
        [EnumMember] File
#else
      Field,
        File
#endif
    }
}