namespace VoipTranslator.Server.Entities
{
    public class User
    {
        public string UserId { get; set; }

        public string Number { get; set; }

        public override int GetHashCode()
        {
            return (UserId ?? "").GetHashCode() ^ (Number ?? "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(UserId) && string.IsNullOrEmpty(user.UserId))
                return Number.Equals(user.Number);

            return UserId.Equals(user.UserId);
        }
    }
}
