using System.Threading.Tasks;
using VoipTranslator.Client.Core.Common;

namespace VoipTranslator.Client.Core
{
    public class AccountManager
    {
        private readonly IKeyValueStorage _storage;
        private const string NumberKey = "Number";

        public AccountManager(IKeyValueStorage storage)
        {
            _storage = storage;
            Number = _storage.GetValue(NumberKey, string.Empty);
        }

        public string Number { get; private set; }

        public bool IsRegistered
        {
            get { return !string.IsNullOrEmpty(Number); }
        }

        public async Task<bool> Register(string number)
        {
            _storage.SetValue(NumberKey, number);
            Number = number;
            return true;
        }
    }
}
