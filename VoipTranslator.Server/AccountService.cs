using System;
using System.Linq;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Server.Domain;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server
{
    public class AccountService
    {
        private readonly CommandBuilder _commandBuilder;
        private readonly ConnectionsManager _connectionsManager;
        private readonly IUsersRepository _usersRepository;

        public AccountService(ConnectionsManager connectionsManager, 
            IUsersRepository usersRepository,
            CommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            _connectionsManager = connectionsManager;
            _usersRepository = usersRepository;
            _connectionsManager.CommandRecieved += _connectionsManager_CommandRecieved;
        }

        private void _connectionsManager_CommandRecieved(object sender, Interfaces.UserCommandEventArgs e)
        {
            switch (e.Command.Name)
            {
                case CommandName.Register:
                    HandleRegistration(e.User, e.Command);
                    break;

                case CommandName.Authentication:
                    HandleAuthentication(e.User, e.Command);
                    break;
            }
        }

        private void HandleRegistration(User user, Command command)
        {
            var request = _commandBuilder.GetUnderlyingObject<RegistrationRequest>(command);
            var result = new RegistrationResult();

            if (string.IsNullOrEmpty(request.Number) ||
                user.Number.Length < 4 ||
                user.Number.Trim(' ', '+').ToCharArray().Any(i => !char.IsDigit(i)))
            {
                result.Result = RegistrationResultType.NotRegistered;
            }
            else
            {
                user.Number = request.Number;
                user.UserId = Guid.NewGuid().ToString();
                _usersRepository.Add(user);
                result.Result = RegistrationResultType.Success;
                result.UserId = user.UserId;
            }
            _commandBuilder.ChangeUnderlyingObject(command, result);
            _connectionsManager.SendCommand(user, command);
        }

        private void HandleAuthentication(User user, Command command)
        {
            var request = _commandBuilder.GetUnderlyingObject<AuthenticationRequest>(command);
            var result = new AuthenticationResult();

            bool exists = _usersRepository.Exists(request.UserId);
            result.Result = exists ? AuthenticationResultType.Success : AuthenticationResultType.NotRegistered;

            _commandBuilder.ChangeUnderlyingObject(command, result);
            _connectionsManager.SendCommand(user, command);
        }
    }
}
