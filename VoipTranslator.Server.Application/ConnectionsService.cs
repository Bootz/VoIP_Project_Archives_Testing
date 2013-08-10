using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VoipTranslator.Infrastructure;
using VoipTranslator.Infrastructure.Logging;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server.Application.Contracts;
using VoipTranslator.Server.Application.Entities;
using VoipTranslator.Server.Application.Entities.EventArguments;
using VoipTranslator.Server.Application.Seedwork;
using VoipTranslator.Server.Domain.Entities.User;
using VoipTranslator.Server.Domain.Seedwork;

namespace VoipTranslator.Server.Application
{
    public class ConnectionsService : AppService
    {
        private static readonly ILogger Logger = LogFactory.GetLogger<ConnectionsService>();
        private readonly ICommandSerializer _serializer;
        private readonly Dictionary<Tuple<string, CommandName>, TaskCompletionSource<Command>> _responceWaiters = new Dictionary<Tuple<string, CommandName>, TaskCompletionSource<Command>>();
        private readonly IUserRepository _userRepository;
        private Timer _timer;
        private readonly List<RemoteUser>_activeConnections = new List<RemoteUser>();
        
        private static readonly TimeSpan TimerInterval = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(1000);

        public ConnectionsService(ICommandsTransportResource resource, 
            ITransactionFactory transactionFactory,
            IUserRepository userRepository,
            ICommandSerializer serializer)
            : base(transactionFactory)
        {
            _userRepository = userRepository;
            _serializer = serializer;
            resource.Received += _resource_OnReceived;
            _timer = new Timer(OnTimerTick, null, TimerInterval);
        }

        public event EventHandler<RemoteUserCommandEventArgs> CommandRecieved = delegate { };

        public RemoteUser FindRemoteUserByNumber(string number)
        {
            lock (_activeConnections)
            {
                var connection = _activeConnections.LastOrDefault(i => i.User.Number == number);
                if (connection == null)
                    return null;
                return connection;
            }
        }

        private void OnTimerTick(object state)
        {
            lock (_activeConnections)
            {
                var timeoutedConnections = _activeConnections.Where(i => (DateTime.Now - i.Peer.LastActivity) > ConnectionTimeout).ToList();
                foreach (var timeoutedConnection in timeoutedConnections)
                {
                    Logger.Trace("Closing connection due to timeout");
                    _activeConnections.Remove(timeoutedConnection);
                }
            }
        }

        private void _resource_OnReceived(object sender, PeerCommandEventArgs e)
        {
            try
            {
                lock (_activeConnections)
                {
                    var command = _serializer.Deserialize(e.Data);
                    if (!string.IsNullOrEmpty(command.UserId))
                    {
                        lock (_responceWaiters)
                        {
                            TaskCompletionSource<Command> taskSource;
                            if (_responceWaiters.TryGetValue(new Tuple<string, CommandName>(command.UserId, command.Name),
                                out taskSource))
                            {
                                taskSource.TrySetResult(command);
                            }
                        }
                    }

                    var user = _userRepository.FirstMatching(UserSpecifications.UserId(command.UserId));
                    RemoteUser remoteUser;
                    if (user != null)
                    {
                        var existedConnection = _activeConnections.FirstOrDefault(i => i.User.Equals(user));
                        if (existedConnection != null)
                        {
                            existedConnection.Peer = e.Peer;
                            existedConnection.Peer.HandleActivity();
                            remoteUser = existedConnection;
                        }
                        else
                        {
                            remoteUser = new RemoteUser(user, e.Peer);
                        }
                        _activeConnections.Add(remoteUser);
                    }
                    else
                    {
                        remoteUser = new RemoteUser(new User(), e.Peer);
                        _activeConnections.Add(remoteUser);
                    }
                    CommandRecieved(this, new RemoteUserCommandEventArgs { Command = command, RemoteUser = remoteUser });
                }
            }
            catch (Exception exc)
            {
                Logger.Exception(exc, "_resource_OnReceived");
            }
        }

        public Task<Command> PostWaiter(string userId, CommandName cmdName)
        {
            lock (_responceWaiters)
            {
                var taskSource = new TaskCompletionSource<Command>();
                _responceWaiters[new Tuple<string, CommandName>(userId, cmdName)] = taskSource;
                return taskSource.Task;
            }
        }
    }
}
