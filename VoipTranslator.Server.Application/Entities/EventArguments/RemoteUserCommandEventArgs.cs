using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Server.Application.Entities.EventArguments
{
    public class RemoteUserCommandEventArgs : CommandEventArgs
    {
        public RemoteUser RemoteUser { get; set; }
    }
}
