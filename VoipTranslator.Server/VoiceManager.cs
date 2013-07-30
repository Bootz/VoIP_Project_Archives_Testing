using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;
using VoipTranslator.Server.Entities;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server
{
    public class VoiceManager
    {
        private readonly CommandBuilder _commandBuilder;
        private readonly ConnectionsManager _connectionsManager;
        private readonly IUsersRepository _usersRepository;

        public VoiceManager(ConnectionsManager connectionsManager, 
            IUsersRepository usersRepository,
            CommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            _connectionsManager = connectionsManager;
            _usersRepository = usersRepository;
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
                dialResult.Result = DialResultType.NotFound;
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
