using System.Threading.Tasks;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Client.Core
{
    public class AccountManager
    {
        private readonly IKeyValueStorage _storage;
        private readonly CommandBuilder _commandBuilder;
        private readonly TransportManager _transportManager;
        private const string NumberKey = "Number";
        private const string UserIdKey = "UserId";

        public AccountManager(IKeyValueStorage storage, 
            CommandBuilder commandBuilder,
            TransportManager transportManager)
        {
            _storage = storage;
            _commandBuilder = commandBuilder;
            _transportManager = transportManager;
            Number = _storage.GetValue(NumberKey, string.Empty);
            UserId = _storage.GetValue(UserIdKey, string.Empty);
        }

        public string Number { get; private set; }

        public string UserId { get; private set; }

        public bool IsRegistered
        {
            get { return !string.IsNullOrEmpty(UserId); }
        }

        public async Task<bool> Register(string number)
        {
            Number = null;
            UserId = null;
            var request = new RegistrationRequest {Number = number};
            var commandRequest = _commandBuilder.Create(CommandName.Register, request);
            var commandReply = await _transportManager.SendCommandAndGetAnswerAsync(commandRequest);
            var result = _commandBuilder.GetUnderlyingObject<RegistrationResult>(commandReply);
            if (result.Result == RegistrationResultType.Success)
            {
                _storage.SetValue(NumberKey, number);
                _storage.SetValue(UserId, result.UserId);
                Number = number;
                UserId = UserId;
                return true;
            }
            return false;
        }
    }
}
