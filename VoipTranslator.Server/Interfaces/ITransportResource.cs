using System;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Server.Interfaces
{
    public interface ITransportResource
    {
        void Send(string data);

        event EventHandler<PacketEventArgs> Received;
    }
}
