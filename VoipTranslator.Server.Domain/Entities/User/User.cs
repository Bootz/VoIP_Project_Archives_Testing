namespace VoipTranslator.Server.Domain.Entities.User
{
    public class User
    {
        public User()
        {
            Number = "";
            UserId = "";
        }

        public string UserId { get; set; }
        public string Number { get; set; }
        public string OsName { get; set; }
        public string PushUri { get; set; }
        public string DeviceName { get; set; }

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
            {
                if (!string.IsNullOrEmpty(Number))
                    return Number.Equals(user.Number);
                return base.Equals(obj);
            }

            return UserId.Equals(user.UserId);
        }
    }
}
