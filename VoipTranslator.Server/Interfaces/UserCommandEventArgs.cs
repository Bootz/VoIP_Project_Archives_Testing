using VoipTranslator.Protocol;
using VoipTranslator.Server.Domain;

namespace VoipTranslator.Server.Interfaces
{
    public class UserCommandEventArgs : CommandEventArgs
    {
        public User User { get; set; }
    }
}
