using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Hammock.Silverlight.Compat
{
#if METRO
    public class Timer : IDisposable
    {
        Action<object> _onTick;
        object _flag1;
        TimeSpan _dueTime;
        TimeSpan _interval;
        bool _shallStop;
        public Timer(Action<object> onTick, object flag1, TimeSpan dueTime, TimeSpan interval)
        {
            _onTick = onTick;
            _flag1 = flag1;
            _dueTime = dueTime;
            _interval = interval;
            _shallStop = false;
            StartTicking();
        }

        async void StartTicking()
        {
            while (!_shallStop)
            {
                await Task.Delay(_interval);
                _onTick(_flag1);
            }
        }

        public void Change(TimeSpan dueTime, TimeSpan interval)
        {
            _dueTime = dueTime;
            _interval = interval;
        }

        public void Change(int dueTime, int interval)
        {
            if (dueTime == -1 || interval == -1)
                _shallStop = true;
            else
            Change(TimeSpan.FromSeconds(dueTime), TimeSpan.FromSeconds(interval));
        }

        public void Dispose()
        {
        }
    }
#endif
}
