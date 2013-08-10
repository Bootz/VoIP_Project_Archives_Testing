using System;
using System.Threading.Tasks;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Client.Core
{
    public class ApplicationManager
    {
        private readonly AccountManager _accountManager;
        private readonly CommandBuilder _commandBuilder;
        private readonly IDeviceInfoProvider _deviceInfo;
        private readonly TransportManager _transportManager;

        public ApplicationManager(
            AccountManager accountManager,
            CommandBuilder commandBuilder,
            IDeviceInfoProvider deviceInfo,
            TransportManager transportManager)
        {
            _accountManager = accountManager;
            _commandBuilder = commandBuilder;
            _deviceInfo = deviceInfo;
            _transportManager = transportManager;
            _transportManager.CommandRecieved += _transportManager_OnCommandRecieved;
        }

        public event EventHandler RegistrationRequested = delegate { }; 

        public async Task StartApp()
        {
            if (!_accountManager.IsRegistered)
                throw new InvalidOperationException();


            var authResult = await Authorize();
            if (authResult.Result == AuthenticationResultType.NotRegistered)
            {
                _accountManager.Deregister();
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
            var token = await _deviceInfo.GetPushUri();
            var osName = await _deviceInfo.GetOsName();
            var deviceName = await _deviceInfo.GetDeviceName();
            var request = new AuthenticationRequest { UserId = _accountManager.UserId, PushUri = token, OsName = osName, DeviceName = deviceName };
            Command requestCmd = _commandBuilder.Create(CommandName.Authentication, request);
            Command replyCmd = await _transportManager.SendCommandAndGetAnswerAsync(requestCmd);
            var result = _commandBuilder.GetUnderlyingObject<AuthenticationResult>(replyCmd);
            return result;
        }
    }
}
