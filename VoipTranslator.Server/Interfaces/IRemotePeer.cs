using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Server.Interfaces
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
