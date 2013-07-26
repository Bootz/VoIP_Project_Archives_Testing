using System;
using System.Globalization;
using System.Threading;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server.Infrastructure
{
    public class TransportResource : ITransportResource
    {
        private readonly DatagramSocket _socket = new DatagramSocket();
        private DataWriter _dataWriter = new DataWriter();
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);

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
            Received(this, new PacketEventArgs { Data = message });
        }

        public async void Send(string data)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                _dataWriter.WriteString(data);
                await _dataWriter.StoreAsync();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public event EventHandler<PacketEventArgs> Received = delegate { };
    }
}
