using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class TransportResource : ITransportResource
    {
        private readonly DatagramSocket _socket = new DatagramSocket();
        private DataWriter _dataWriter = new DataWriter();
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);
        private bool _isServiceBound = false;

        public TransportResource()
        {
            _socket.MessageReceived += _socket_OnMessageReceived;
        }

        private async Task BindService()
        {
            var stream = await _socket.GetOutputStreamAsync(
                new HostName(ServerAddress.HostName),
                ServerAddress.Port.ToString(CultureInfo.InvariantCulture));
            _dataWriter = new DataWriter(stream);
        }

        private void _socket_OnMessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs e)
        {
            var reader = e.GetDataReader();
            string message = reader.ReadString(reader.UnconsumedBufferLength);
            Received(this, new PacketEventArgs {Data = message});
        }

        public async void Send(string data)
        {
            try
            {
                await SemaphoreSlim.WaitAsync();
                if (!_isServiceBound)
                {
                    _isServiceBound = true;
                    await BindService();
                }
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
