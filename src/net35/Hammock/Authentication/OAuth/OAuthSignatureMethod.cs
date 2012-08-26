using System;
using System.Runtime.Serialization;

namespace Hammock.Authentication.OAuth
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public enum OAuthSignatureMethod
    {
#if !SILVERLIGHT && !PORTABLE && !Smartphone && !ClientProfiles && !NET20 && !MonoTouch && !NETCF
        [EnumMember] HmacSha1,
        [EnumMember] PlainText,
        [EnumMember] RsaSha1
#else
      HmacSha1,
        PlainText,
        RsaSha1
#endif
    }
}