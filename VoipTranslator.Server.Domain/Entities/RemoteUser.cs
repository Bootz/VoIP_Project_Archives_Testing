namespace VoipTranslator.Server.Domain.Entities
{
    public class RemoteUser
    {
        public User User { get; set; }
        public IRemotePeer Peer { get; set; }
        public RemoteUser IsInCallWith { get; set; }

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

            if (User == null || string.IsNullOrEmpty(User.UserId))
                return Peer.Equals(ru.Peer);

            return ru.User.Equals(User);
        }

        public override int GetHashCode()
        {
            if (User == null || string.IsNullOrEmpty(User.UserId))
                return Peer.GetHashCode();

            return User.GetHashCode();
        }
    }
}
