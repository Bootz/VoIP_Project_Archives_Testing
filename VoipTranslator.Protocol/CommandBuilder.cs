using VoipTranslator.Protocol.Serializers;

namespace VoipTranslator.Protocol
{
    public class CommandBuilder
    {
        private readonly IDtoSerializer _dtoSerializer;

        public CommandBuilder(IDtoSerializer dtoSerializer)
        {
            _dtoSerializer = dtoSerializer;
        }

        public Command Create<T>(CommandName name, T body)
        {
            string bodyString = _dtoSerializer.Serialize(body);
            return new Command(name, bodyString);
        }

        public void ChangeUnderlyingObject<T>(Command cmd, T obj)
        {
            string bodyString = _dtoSerializer.Serialize(obj);
            cmd.Body = bodyString;
        }

        public T GetUnderlyingObject<T>(Command cmd)
        {
            return _dtoSerializer.Deserialize<T>(cmd.Body);
        }
    }
}
