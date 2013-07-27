using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server.Domain;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server
{
    public class ConnectionsManager
    {
        private readonly ICommandSerializer _serializer;
        private readonly Dictionary<long, TaskCompletionSource<Command>> _responseWaiters = new Dictionary<long, TaskCompletionSource<Command>>();
        private readonly ITransportResource _resource;
        private readonly IUsersRepository _userRepository;
        private Timer _timer;
        private readonly Dictionary<User, DateTime> _activeConnections = new Dictionary<User, DateTime>();
        
        private static readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(30);

        public ConnectionsManager(ITransportResource resource, 
            IUsersRepository userRepository,
            ICommandSerializer serializer)
        {
            _resource = resource;
            _userRepository = userRepository;
            _serializer = serializer;
            _resource.Received += _resource_OnReceived;
            _timer = new Timer(OnTimerTick, null, (int)TimerInterval.TotalMilliseconds, (int)TimerInterval.TotalMilliseconds);
        }

        public event EventHandler<UserCommandEventArgs> CommandRecieved = delegate { };

        public IReadOnlyCollection<User> ActiveConnections
        {
            get
            {
                lock (_activeConnections)
                {
                    return new ReadOnlyCollection<User>(_activeConnections.Keys.ToList()); 
                }
            }
        }

        public void SendCommand(User user, Command cmd)
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

        public Task SendCommandAndWaitAckAsync(User user, Command cmd)
        {
            return null;
        }

        private void OnTimerTick(object state)
        {
            lock (_activeConnections)
            {
                var timeoutedConnections = _activeConnections.Where(i => (DateTime.Now - i.Value) > ConnectionTimeout).ToList();
                foreach (var timeoutedConnection in timeoutedConnections)
                {
                    _activeConnections.Remove(timeoutedConnection.Key);
                }
            }
        }

        private void _resource_OnReceived(object sender, PacketEventArgs e)
        {
            try
            {
                lock (_responseWaiters)
                {
                    var command = _serializer.Deserialize(e.Data);
                    var user = _userRepository.GetById(command.UserId);
                    if (user != null)
                    {
                        _activeConnections[user] = DateTime.Now;
                    }
                    else
                    {
                        user = new User();
                    }
                    user.Address = e.RemotePeerAddress;
                    user.Port = e.RemotePeerPort;
                    CommandRecieved(this, new UserCommandEventArgs { Command = command, User = user });
                }
            }
            catch (Exception)
            {
                //Log
            }
        }
    }
}
