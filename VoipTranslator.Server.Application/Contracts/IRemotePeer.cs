using System;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Server.Application.Contracts
{
    public interface IRemotePeer
    {
        Task SendCommand(Command command);

        Task<Command> SendCommandAndWaitAnswer(Command command);

        string HostName { get; }

        string Port { get; }

        DateTime LastActivity { get; }

        void HandleActivity();
    }
}
