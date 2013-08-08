using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Server.Application
{
    public class PeerCommandEventArgs : PacketEventArgs
    {
        public IRemotePeer Peer { get; set; }
    }
}