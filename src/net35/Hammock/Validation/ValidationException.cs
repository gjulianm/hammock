using System;

namespace Hammock.Validation
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class ValidationException : Exception
    {
        public ValidationException()
        {

        }

        public ValidationException(string message) : base(message)
        {

        }
    }
}