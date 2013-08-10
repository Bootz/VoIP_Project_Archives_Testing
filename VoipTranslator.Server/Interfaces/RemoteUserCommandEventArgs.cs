using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Server.Entities;

namespace VoipTranslator.Server.Interfaces
{
    public class RemoteUserCommandEventArgs : CommandEventArgs
    {
        public RemoteUser RemoteUser { get; set; }
    }
}
