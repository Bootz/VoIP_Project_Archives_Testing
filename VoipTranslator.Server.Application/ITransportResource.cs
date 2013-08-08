using System;

namespace VoipTranslator.Server.Application
{
    public interface ITransportResource
    {
        event EventHandler<PeerCommandEventArgs> Received;
    }
}
