using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Server.Application.Contracts;
using VoipTranslator.Server.Application.Entities;
using VoipTranslator.Server.Application.Entities.EventArguments;
using VoipTranslator.Server.Domain.Entities.User;

namespace VoipTranslator.Server.Application
{
    public class VoiceManager
    {
        private readonly CommandBuilder _commandBuilder;
        private readonly ConnectionsService _connectionsManager;
        private readonly IUserRepository _usersRepository;
        private readonly IPushNotificationResource _pushSender;

        public VoiceManager(ConnectionsService connectionsManager,
            IUserRepository usersRepository,
            IPushNotificationResource pushSender,
            CommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            _connectionsManager = connectionsManager;
            _usersRepository = usersRepository;
            _pushSender = pushSender;
            _connectionsManager.CommandRecieved += _connectionsManager_OnCommandRecieved;
        }

        private void _connectionsManager_OnCommandRecieved(object sender, RemoteUserCommandEventArgs e)
        {
            switch (e.Command.Name)
            {
                case CommandName.Dial:
                    HandleDial(e.Command, e.RemoteUser);
                    break;
                case CommandName.VoicePacket:
                    HandleVoicePacket(e.Command, e.RemoteUser);
                    break;
                case CommandName.EndCall:
                    HandleEndCall(e.Command, e.RemoteUser);
                    break;
            }
        }

        private void HandleVoicePacket(Command command, RemoteUser remoteUser)
        {
            if (remoteUser.IsInCallWith == null)
                return;

            remoteUser.IsInCallWith.Peer.SendCommand(command);
        }

        private void HandleEndCall(Command command, RemoteUser remoteUser)
        {
            if (remoteUser.IsInCallWith == null)
                return;

            var cmd = _commandBuilder.Create(CommandName.EndCall, string.Empty);
            remoteUser.IsInCallWith.Peer.SendCommand(cmd);
        }

        private async void HandleDial(Command command, RemoteUser remoteUser)
        {
            var callerNumber = _commandBuilder.GetUnderlyingObject<string>(command);
            var opponent = _connectionsManager.FindRemoteUserByNumber(callerNumber);

            var dialResult = new DialResult();

            if (opponent == null)
            {
                var opponentUser = _usersRepository.FirstMatching(UserSpecifications.Number(callerNumber));
                if (opponentUser != null)
                {
                    _pushSender.SendVoipPush(opponentUser.PushUri, remoteUser.User.Number, remoteUser.User.Number);
                    var resultCommand = await _connectionsManager.PostWaiter(opponentUser.UserId, CommandName.IncomingCall);
                    var answerType = _commandBuilder.GetUnderlyingObject<AnswerResultType>(resultCommand);
                    if (answerType == AnswerResultType.Answered)
                    {
                        dialResult.Result = DialResultType.Answered;
                        opponent.IsInCallWith = remoteUser;
                        remoteUser.IsInCallWith = opponent;
                    }
                    else
                    {
                        dialResult.Result = DialResultType.Declined;
                    }
                }
                else
                {
                    dialResult.Result = DialResultType.NotFound;
                }
            }
            else
            {
                var incomingCallCommand = _commandBuilder.Create(CommandName.IncomingCall, callerNumber);
                var resultCommand = await opponent.Peer.SendCommandAndWaitAnswer(incomingCallCommand);
                var answerType = _commandBuilder.GetUnderlyingObject<AnswerResultType>(resultCommand);
                if (answerType == AnswerResultType.Answered)
                {
                    dialResult.Result = DialResultType.Answered;
                    opponent.IsInCallWith = remoteUser;
                    remoteUser.IsInCallWith = opponent;
                }
                else
                {
                    dialResult.Result = DialResultType.Declined;
                }
            }
            _commandBuilder.ChangeUnderlyingObject(command, dialResult);
            remoteUser.Peer.SendCommand(command);
        }
    }
}
