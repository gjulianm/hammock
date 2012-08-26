using System;
using System.Diagnostics;

namespace Hammock.Web
{
#if !Smartphone && !NETCF
    [DebuggerDisplay("{Name}:{Value}")]
#endif
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class WebHeader : WebPair
    {
        public WebHeader(string name, string value) : base(name, value)
        {
        }
    }
}