using System;
using VoipTranslator.Server.Application.Entities.EventArguments;

namespace VoipTranslator.Server.Application.Contracts
{
    public interface ICommandsTransportResource
    {
        event EventHandler<PeerCommandEventArgs> Received;
    }
}
