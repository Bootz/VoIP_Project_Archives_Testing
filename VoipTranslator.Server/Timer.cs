using System;
using System.Threading.Tasks;

namespace VoipTranslator.Server
{
    internal delegate void TimerCallback(object state);

    internal sealed class Timer
    {
        private readonly TimerCallback _callback;
        private readonly object _state;
        private readonly TimeSpan _interval;

        internal Timer(TimerCallback callback, object state, TimeSpan interval)
        {
            _callback = callback;
            _state = state;
            _interval = interval;
            StartTimer();
        }

        private async void StartTimer()
        {
            while (true)
            {
                await Task.Delay(_interval).ConfigureAwait(false);
                _callback(_state);
            }
        }
    }
}