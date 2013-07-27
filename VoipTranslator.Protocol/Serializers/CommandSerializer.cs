namespace VoipTranslator.Protocol.Serializers
{
    public class CommandSerializer : ICommandSerializer
    {
        private readonly XmlDtoSerializer _xmlSerializer = new XmlDtoSerializer();

        public Command Deserialize(string text)
        {
            return _xmlSerializer.Deserialize<Command>(text);
        }

        public string Serialize(Command cmd)
        {
            return _xmlSerializer.Serialize(cmd);
        }
    }
}
