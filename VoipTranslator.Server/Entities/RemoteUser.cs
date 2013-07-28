using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server.Entities
{
    public class RemoteUser
    {
        public User User { get; set; }
        public IRemotePeer Peer { get; set; }

        public RemoteUser(User user, IRemotePeer peer)
        {
            User = user;
            Peer = peer;
        }

        public override bool Equals(object obj)
        {
            var ru = obj as RemoteUser;
            if (ru == null)
                return false;
            return ru.User.Equals(User);
        }

        public override int GetHashCode()
        {
            return User.GetHashCode();
        }
    }
}
