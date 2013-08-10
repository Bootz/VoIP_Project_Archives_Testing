using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Contracts;

namespace VoipTranslator.Server.Infrastructure
{
    public class EmptyUserIdProvider : IUserIdProvider
    {
        public string UserId { get { return string.Empty; } set {} }
    }
}
