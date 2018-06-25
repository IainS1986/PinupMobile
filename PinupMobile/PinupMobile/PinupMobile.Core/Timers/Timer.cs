using System;
using System.Threading;
using System.Threading.Tasks;

namespace PinupMobile.Core.Timers
{
    public class Timer
    {
        private bool _started;

        public int Time { get; private set; }
        public event EventHandler<int> TimeElapsed;

        public async void Start(CancellationToken token = default(CancellationToken))
        {
            if (_started) 
                return;

            _started = true;
            Time = 0;

            while (_started)
            {
                // wait 1000 ms
                await Task.Delay(1000, token).ConfigureAwait(false);
                TimeElapsed?.Invoke(this, ++Time);
            }
        }

        public void Stop()
        {
            _started = false;
        }
    }
}
