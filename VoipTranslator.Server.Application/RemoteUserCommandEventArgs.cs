using VoipTranslator.Protocol;

namespace VoipTranslator.Server.Application
{
    public class RemoteUserCommandEventArgs : CommandEventArgs
    {
        public RemoteUser RemoteUser { get; set; }
    }
}
