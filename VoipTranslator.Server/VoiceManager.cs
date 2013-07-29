using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    e.RemoteUser.Peer.SendCommand(e.Command);
                    break;
            }
        }

        private async void HandleDial(Command command, RemoteUser remoteUser)
        {
            var callerNumber = _commandBuilder.GetUnderlyingObject<string>(command);
            var peer = _connectionsManager.FindRemoteUserByNumber(callerNumber);

            var dialResult = new DialResult();

            if (peer == null)
            {
                dialResult.Result = DialResultType.NotFound;
            }
            else
            {
                await Task.Delay(3000);
                dialResult.Result = DialResultType.Answered;
            }
            _commandBuilder.ChangeUnderlyingObject(command, dialResult);
            remoteUser.Peer.SendCommand(command);
        }
    }
}
