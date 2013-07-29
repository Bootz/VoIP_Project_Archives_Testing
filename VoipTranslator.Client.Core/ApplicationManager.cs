using System;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Client.Core
{
    public class ApplicationManager
    {
        private readonly AccountManager _accountManager;
        private readonly CommandBuilder _commandBuilder;
        private readonly TransportManager _transportManager;

        public ApplicationManager(
            AccountManager accountManager,
            CommandBuilder commandBuilder,
            TransportManager transportManager)
        {
            _accountManager = accountManager;
            _commandBuilder = commandBuilder;
            _transportManager = transportManager;
            _transportManager.CommandRecieved += _transportManager_OnCommandRecieved;
        }

        public event EventHandler RegistrationRequested = delegate { }; 

        public async void StartApp()
        {
            if (!_accountManager.IsRegistered)
                throw new InvalidOperationException();

            var authResult = await Authorize();
            if (authResult.Result == AuthenticationResultType.NotRegistered)
            {
                RegistrationRequested(this, EventArgs.Empty);
                return;
            }
        }

        private void _transportManager_OnCommandRecieved(object sender, CommandEventArgs e)
        {
            //switch (e.Command.Name)
            //{
            //}
        }

        private async Task<AuthenticationResult> Authorize()
        {
            var request = new AuthenticationRequest { UserId = _accountManager.UserId };
            Command requestCmd = _commandBuilder.Create(CommandName.Authentication, request);
            Command replyCmd = await _transportManager.SendCommandAndGetAnswerAsync(requestCmd);
            var result = _commandBuilder.GetUnderlyingObject<AuthenticationResult>(replyCmd);
            return result;
        }
    }
}
