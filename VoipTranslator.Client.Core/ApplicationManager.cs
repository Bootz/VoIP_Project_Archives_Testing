using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Client.Core
{
    public class ApplicationManager
    {
        private readonly AccountManager _accountManager;
        private readonly TransportManager _transportManager;

        public ApplicationManager(AccountManager accountManager, TransportManager transportManager)
        {
            _accountManager = accountManager;
            _transportManager = transportManager;
        }

        private Task<AuthenticationResult> Authorize()
        {
            _transportManager.SendCommandAndGetAnswerAsync(new Command(CommandName.Authorize, ))
        }
    }
}
