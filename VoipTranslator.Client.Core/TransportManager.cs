using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.Core
{
    public class TransportManager
    {
        private readonly ITransportResource _resource;
        private readonly ICommandSerializer _serializer;
        private readonly Dictionary<long, TaskCompletionSource<Command>> _responseWaiters = new Dictionary<long,TaskCompletionSource<Command>>();

        public TransportManager(ITransportResource resource, ICommandSerializer serializer)
        {
            _resource = resource;
            _serializer = serializer;
            _resource.Received += _resource_OnReceived;
        }

        private void _resource_OnReceived(object sender, PacketEventArgs e)
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
                    else
                    {
                        //Log
                    }
                }
            }
            catch (Exception)
            {
                //Log
            }
        }

        public Task<Command> SendCommandAndGetAnswerAsync(Command cmd)
        {
            var taskSource = new TaskCompletionSource<Command>();
            try
            {
                lock (_responseWaiters)
                {
                    var data = _serializer.Serialize(cmd);
                    _responseWaiters[cmd.PacketId] = taskSource;
                    _resource.Send(data);
                }
            }
            catch (Exception)
            {
                throw;
                //Log
            }
            return taskSource.Task;
        }

        public void SendCommand(Command cmd)
        {
            try
            {
                var data = _serializer.Serialize(cmd);
                _resource.Send(data);
            }
            catch (Exception)
            {
                //Log
            }
        }
    }
}
