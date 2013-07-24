using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.Core
{
    public class TransportManager
    {
        private readonly ITransportResource _resource;
        private readonly ICommandSerializer _serializer;

        public TransportManager(ITransportResource resource, ICommandSerializer serializer)
        {
            _resource = resource;
            _serializer = serializer;
            _resource.Received += _resource_OnReceived;
        }

        private void _resource_OnReceived(object sender, PacketEventArgs e)
        {
            var command = _serializer.Deserialize(e.Data);
        }

        public Task<Command> SendCommandAndGetAnswerAsync(Command cmd)
        {
            var data = _serializer.Serialize(cmd);
            _resource.Send(data);
        }

        public void SendCommand(Command cmd)
        {
            var data = _serializer.Serialize(cmd);
            _resource.Send(data);
        }
    }
}
