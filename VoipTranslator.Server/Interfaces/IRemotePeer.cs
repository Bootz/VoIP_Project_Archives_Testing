using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Protocol;

namespace VoipTranslator.Server.Interfaces
{
    public interface IRemotePeer
    {
        Task SendCommand(Command command);

        string HostName { get; }

        string Port { get; }
    }
}
