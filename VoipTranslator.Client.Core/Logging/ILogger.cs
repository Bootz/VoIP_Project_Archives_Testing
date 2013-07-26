using System;

namespace VoipTranslator.Client.Core.Logging
{
    public interface ILogger
    {
        void Trace(string format, params object[] args);

        void Debug(string format, params object[] args);

        void Info(string format, params object[] args);

        void Warn(string format, params object[] args);

        void Error(string format, params object[] args);

        void Exception(Exception exc);
        
        void Exception(Exception exc, string format, params object[] args);
    }
}
