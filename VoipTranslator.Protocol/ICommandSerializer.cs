namespace VoipTranslator.Protocol
{
    public interface ICommandSerializer
    {
        Command Deserialize(string text);
        string Serialize(Command cmd);
    }
}