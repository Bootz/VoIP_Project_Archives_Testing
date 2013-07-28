namespace VoipTranslator.Protocol.Dto
{
    public class RegistrationRequest
    {
        public string Number { get; set; }

        public string DeviceName { get; set; }

        public string OsVersion { get; set; }

        public string PushUri { get; set; }
    }
}