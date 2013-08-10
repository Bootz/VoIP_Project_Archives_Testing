using VoipTranslator.Protocol.Commands;

namespace VoipTranslator.Protocol.Serializers
{
    public interface ICommandSerializer
    {
        Command Deserialize(string text);

        string Serialize(Command cmd);
    }
}