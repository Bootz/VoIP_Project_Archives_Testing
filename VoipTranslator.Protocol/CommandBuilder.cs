using VoipTranslator.Protocol.Serializers;

namespace VoipTranslator.Protocol
{
    public class CommandBuilder
    {
        private readonly IDtoSerializer _dtoSerializer;
        private readonly IUserIdProvider _userIdprovider;

        public CommandBuilder(IDtoSerializer dtoSerializer, IUserIdProvider userIdprovider)
        {
            _dtoSerializer = dtoSerializer;
            _userIdprovider = userIdprovider;
        }

        public Command Create<T>(CommandName name, T body)
        {
            string bodyString = _dtoSerializer.Serialize(body);
            return new Command(name, bodyString) { UserId = _userIdprovider.UserId };
        }

        public void ChangeUnderlyingObject<T>(Command cmd, T obj)
        {
            string bodyString = _dtoSerializer.Serialize(obj);
            cmd.UserId = _userIdprovider.UserId;
            cmd.Body = bodyString;
        }

        public T GetUnderlyingObject<T>(Command cmd)
        {
            return _dtoSerializer.Deserialize<T>(cmd.Body);
        }
    }
}
