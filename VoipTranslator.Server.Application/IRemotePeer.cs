using System;
using System.Threading.Tasks;
using VoipTranslator.Protocol;

namespace VoipTranslator.Server.Application
{
    public interface IRemotePeer
    {
        Task SendCommand(Command command);

        Task<Command> SendCommandAndWaitAnswer(Command command);

        string HostName { get; }

        string Port { get; }

        DateTime LastActivity { get; }

        void UpdateLastActivity();
    }
}
