using System;

namespace Hammock.Caching
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class CacheOptions
    {
        public virtual CacheMode Mode { get; set; }
        public virtual TimeSpan Duration { get; set; }
    }
}
