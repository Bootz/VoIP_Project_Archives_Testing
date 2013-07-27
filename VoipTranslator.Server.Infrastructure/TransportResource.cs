using System;
using System.Globalization;
using System.Threading;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Server.Interfaces;
using VoipTranslator.Server.Logging;

namespace VoipTranslator.Server.Infrastructure
{
    public class TransportResource : ITransportResource
    {
        private readonly DatagramSocket _socket = new DatagramSocket();
        private DataWriter _dataWriter = new DataWriter();
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);
        private static readonly ILogger Logger = LogFactory.GetLogger<TransportResource>();

        public TransportResource()
        {
            _socket.MessageReceived += _socket_OnMessageReceived;
            BindService();
        }

        private async void BindService()
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                _socket.BindServiceNameAsync(ServerAddress.HostName);
                var stream = await _socket.GetOutputStreamAsync(
                    new HostName(ServerAddress.HostName),
                    ServerAddress.Port.ToString(CultureInfo.InvariantCulture));
                _dataWriter = new DataWriter(stream);
                Logger.Debug("BindService completed");
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        private void _socket_OnMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs e)
        {
            var reader = e.GetDataReader();
            string message = reader.ReadString(reader.UnconsumedBufferLength);

            Logger.Trace("Message from {0}:{1}:\n{2}", e.RemoteAddress.RawName, e.RemotePort, message);
            Received(this, new PacketEventArgs { Data = message, RemotePeerAddress = e.RemoteAddress.RawName, RemotePeerPort = e.RemotePort });
        }

        public async void Send(string data)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                _dataWriter.WriteString(data);
                await _dataWriter.StoreAsync();
                Logger.Trace("Message sent :\n{0}", data);
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public event EventHandler<PacketEventArgs> Received = delegate { };
    }
}
