using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Hammock.Silverlight.Compat
{
    public class AwaitAsyncResult : IAsyncResult
    {
        public AwaitAsyncResult(object userState)
        {
            AsyncState = userState;
        }

        public object AsyncState { get; protected set; }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return new EventWaitHandle(true, EventResetMode.ManualReset);
            }
        }

        public bool CompletedSynchronously { get { return true; } }

        public bool IsCompleted { get { return true; } }
    }
}
