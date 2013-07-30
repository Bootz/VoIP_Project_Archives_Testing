using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server.Interfaces;
using VoipTranslator.Server.Logging;
using Windows.Networking;
using Windows.Storage.Streams;

namespace VoipTranslator.Server.Infrastructure
{
    public class RemotePeer : IRemotePeer
    {
        private readonly ICommandSerializer _serializer;
        private readonly ITransportResource _transport;
        private readonly DataWriter _dataWriter;
        private static readonly ILogger Logger = LogFactory.GetLogger<RemotePeer>();
        private readonly Dictionary<long, TaskCompletionSource<Command>> _responseWaiters = new Dictionary<long, TaskCompletionSource<Command>>();

        public RemotePeer(ICommandSerializer serializer, ITransportResource transport, IOutputStream stream, HostName host, string port)
        {
            _serializer = serializer;
            _transport = transport;
            _dataWriter = new DataWriter(stream);
            HostName = host.RawName;
            Port = port;
            UpdateLastActivity();
            _transport.Received += _transport_Received;
        }

        void _transport_Received(object sender, PeerCommandEventArgs e)
        {
            try
            {
                lock (_responseWaiters)
                {
                    var command = _serializer.Deserialize(e.Data);
                    TaskCompletionSource<Command> taskSource;
                    if (_responseWaiters.TryGetValue(command.PacketId, out taskSource))
                    {
                        taskSource.TrySetResult(command);
                    }
                }
            }
            catch (Exception)
            {
                //Log
            }
        }

        public async Task SendCommand(Command command)
        {
            try
            {
                var message = _serializer.Serialize(command);
                Logger.Trace("Sending data to {0}:{1}:  {2}", HostName, Port, message);
                _dataWriter.WriteString(message);
                await _dataWriter.StoreAsync();
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "During send data to {0}:{1}", HostName, Port);
                throw;
            }
        }

        public Task<Command> SendCommandAndWaitAnswer(Command command)
        {
            var taskSource = new TaskCompletionSource<Command>();
            try
            {
                lock (_responseWaiters)
                {
                    _responseWaiters[command.PacketId] = taskSource;
                }
                SendCommand(command);
            }
            catch (Exception)
            {
                throw;
                //Log
            }
            return taskSource.Task;
        }

        public string HostName { get; private set; }

        public string Port { get; private set; }

        public DateTime LastActivity { get; private set; }

        public void UpdateLastActivity()
        {
            LastActivity = DateTime.Now;
        }

        public override int GetHashCode()
        {
            return HostName.GetHashCode() ^ Port.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var peer = obj as RemotePeer;
            if (peer == null)
                return false;
            return peer.HostName == HostName && peer.Port == Port;
        }
    }
}