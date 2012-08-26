using System;
using System.Text;

namespace Hammock.Serialization
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class Utf8Serializer
    {
        public virtual Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }
    }
}