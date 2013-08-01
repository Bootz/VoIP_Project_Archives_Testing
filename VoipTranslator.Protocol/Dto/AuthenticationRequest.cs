namespace VoipTranslator.Protocol.Dto
{
    public class AuthenticationRequest
    {
        public string PushUri { get; set; }

        public string UserId { get; set; }
        
        public string DeviceName { get; set; }

        public string OsName { get; set; }
    }
}
