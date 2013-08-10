using System;
using System.Globalization;
using VoipTranslator.Infrastructure.Logging;
using VoipTranslator.Protocol.Serializers;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using VoipTranslator.Protocol;
using VoipTranslator.Server.Application.Contracts;
using VoipTranslator.Server.Application.Entities.EventArguments;

namespace VoipTranslator.Server.Infrastructure
{
    public class TransportResource : ICommandsTransportResource
    {
        private readonly ICommandSerializer _serializer;
        private readonly DatagramSocket _socket = new DatagramSocket();
        private static readonly ILogger Logger = LogFactory.GetLogger<TransportResource>();

        public TransportResource(ICommandSerializer serializer)
        {
            _serializer = serializer;
            _socket.MessageReceived += _socket_OnMessageReceived;
            BindService();
        }

        private async void BindService()
        {
            await _socket.BindServiceNameAsync(ServerAddress.Port.ToString(CultureInfo.InvariantCulture));
            Logger.Debug("BindService completed");
        }

        private async void _socket_OnMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs e)
        {
            var reader = e.GetDataReader();
            string message = reader.ReadString(reader.UnconsumedBufferLength);

            Logger.Trace("Message from {0}:{1}:\n{2}", e.RemoteAddress.RawName, e.RemotePort, message);

            IOutputStream outputStream = await sender.GetOutputStreamAsync(e.RemoteAddress, e.RemotePort);
            Received(this, new PeerCommandEventArgs
                               {
                                   Data = message,
                                   Peer = new RemotePeer(_serializer, this, outputStream, e.RemoteAddress, e.RemotePort),
                               });
        }

        public event EventHandler<PeerCommandEventArgs> Received = delegate { };
    }
}
