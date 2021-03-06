﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Storage.Streams;
using VoipTranslator.Infrastructure.Logging;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server.Application.Contracts;
using VoipTranslator.Server.Application.Entities.EventArguments;

namespace VoipTranslator.Server.Infrastructure.Network
{
    public class RemotePeer : IRemotePeer
    {
        private readonly ICommandSerializer _serializer;
        private readonly ICommandsTransportResource _transport;
        private readonly DataWriter _dataWriter;
        private static readonly ILogger Logger = LogFactory.GetLogger<RemotePeer>();
        private readonly Dictionary<string, TaskCompletionSource<Command>> _responseWaiters = new Dictionary<string, TaskCompletionSource<Command>>();

        public RemotePeer(ICommandSerializer serializer, ICommandsTransportResource transport, IOutputStream stream, HostName host, string port)
        {
            _serializer = serializer;
            _transport = transport;
            _dataWriter = new DataWriter(stream);
            HostName = host.RawName;
            Port = port;
            HandleActivity();
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

        public void HandleActivity()
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