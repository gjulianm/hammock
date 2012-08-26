using System;

namespace Hammock.Tasks
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class TaskState : IRetryState
    {
        #region ITaskState Members

        public int RepeatCount
        {
            get;set;
        }

        public DateTime? LastRepeat
        {
            get; set;
        }

        #endregion
    }
}
