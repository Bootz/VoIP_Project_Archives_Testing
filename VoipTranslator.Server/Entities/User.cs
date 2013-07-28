namespace VoipTranslator.Server.Entities
{
    public class User
    {
        public string UserId { get; set; }

        public string Number { get; set; }
        
        public string Address { get; set; }

        public string Port { get; set; }

        public override int GetHashCode()
        {
            return UserId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var user = obj as User;
            if (user == null)
                return false;
            return UserId.Equals(user.UserId);
        }
    }
}
