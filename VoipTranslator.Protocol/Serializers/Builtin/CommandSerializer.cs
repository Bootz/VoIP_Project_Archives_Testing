namespace VoipTranslator.Protocol.Serializers.Builtin
{
    public class CommandSerializer : ICommandSerializer
    {
        private readonly IDtoSerializer _innerSerializer = new DcDtoSerializer();

        public Command Deserialize(string text)
        {
            return _innerSerializer.Deserialize<Command>(text);
        }

        public string Serialize(Command cmd)
        {
            return _innerSerializer.Serialize(cmd);
        }
    }
}
