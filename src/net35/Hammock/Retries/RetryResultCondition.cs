using System;
using Hammock.Web;

namespace Hammock.Retries
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public abstract class RetryResultCondition : IRetryCondition<WebQueryResult>
    {
        public virtual Predicate<WebQueryResult> RetryIf
        {
            get { return r => false; }
        }
    }
}