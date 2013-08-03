using System;

namespace VoipTranslator.Server.Domain.Seedwork
{
    //Portable doesn't have any timer
    public interface ITimer
    {
        void Start(TimeSpan interval);
        void Stop();
        event EventHandler Tick;
    }
}
