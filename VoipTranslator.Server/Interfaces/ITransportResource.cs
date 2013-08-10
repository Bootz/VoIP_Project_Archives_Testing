using System;

namespace VoipTranslator.Server.Interfaces
{
    public interface ITransportResource
    {
        event EventHandler<PeerCommandEventArgs> Received;
    }
}
