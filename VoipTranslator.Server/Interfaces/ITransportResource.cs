using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Server.Interfaces
{
    public interface ITransportResource
    {
        void Send(string data);

        event EventHandler<PacketEventArgs> Received;
    }
}
