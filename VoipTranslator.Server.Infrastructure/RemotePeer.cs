using System;
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
        private readonly DataWriter _dataWriter;
        private static readonly ILogger Logger = LogFactory.GetLogger<RemotePeer>();

        public RemotePeer(ICommandSerializer serializer, IOutputStream stream, HostName host, string port)
        {
            _serializer = serializer;
            _dataWriter = new DataWriter(stream);
            HostName = host.RawName;
            Port = port;
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

        public string HostName { get; private set; }

        public string Port { get; private set; }
    }
}