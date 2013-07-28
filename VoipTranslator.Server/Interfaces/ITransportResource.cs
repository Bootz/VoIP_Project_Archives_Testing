using System;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Server.Interfaces
{
    public interface ITransportResource
    {
        event EventHandler<PeerCommandEventArgs> Received;
    }
}
