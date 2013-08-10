using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Contracts;

namespace VoipTranslator.Client.Core.Common
{
    public class UserIdProvider : IUserIdProvider
    {
        public string UserId { get; set; }
    }
}
