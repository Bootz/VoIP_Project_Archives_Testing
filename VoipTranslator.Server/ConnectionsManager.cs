using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server.Entities;
using VoipTranslator.Server.Interfaces;
using VoipTranslator.Server.Logging;

namespace VoipTranslator.Server
{
    public class ConnectionsManager
    {
        private static readonly ILogger Logger = LogFactory.GetLogger<ConnectionsManager>();
        private readonly ICommandSerializer _serializer;
        private readonly Dictionary<long, TaskCompletionSource<Command>> _responseWaiters = new Dictionary<long, TaskCompletionSource<Command>>();
        private readonly ITransportResource _resource;
        private readonly IUsersRepository _userRepository;
        private Timer _timer;
        private readonly Dictionary<RemoteUser, DateTime> _activeConnections = new Dictionary<RemoteUser, DateTime>();
        
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
            _timer = new Timer(OnTimerTick, null, TimerInterval);
        }

        public event EventHandler<RemoteUserCommandEventArgs> CommandRecieved = delegate { };

        public IReadOnlyCollection<RemoteUser> ActiveConnections
        {
            get
            {
                lock (_activeConnections)
                    return new ReadOnlyCollection<RemoteUser>(_activeConnections.Keys.ToList()); 
            }
        }

        private void OnTimerTick(object state)
        {
            lock (_activeConnections)
            {
                var timeoutedConnections = _activeConnections.Where(i => (DateTime.Now - i.Value) > ConnectionTimeout).ToList();
                foreach (var timeoutedConnection in timeoutedConnections)
                {
                    Logger.Trace("Closing connection due to timeout");
                    _activeConnections.Remove(timeoutedConnection.Key);
                }
            }
        }

        private void _resource_OnReceived(object sender, PeerCommandEventArgs e)
        {
            try
            {
                lock (_responseWaiters)
                {
                    var command = _serializer.Deserialize(e.Data);
                    var user = _userRepository.GetById(command.UserId);
                    RemoteUser remoteUser;
                    if (user != null)
                    {
                        remoteUser = new RemoteUser(user, e.Peer);
                        _activeConnections[remoteUser] = DateTime.Now;
                    }
                    else
                    {
                        remoteUser = new RemoteUser(new User(), e.Peer);
                    }
                    CommandRecieved(this, new RemoteUserCommandEventArgs { Command = command, RemoteUser = remoteUser });
                }
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "_resource_OnReceived");
            }
        }
    }
}
