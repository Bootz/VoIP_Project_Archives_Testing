using VoipTranslator.Protocol;
using VoipTranslator.Server.Application.Contracts;

namespace VoipTranslator.Server.Application.Entities.EventArguments
{
    public class PeerCommandEventArgs : PacketEventArgs
    {
        public IRemotePeer Peer { get; set; }
    }
}