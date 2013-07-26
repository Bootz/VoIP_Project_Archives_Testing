namespace VoipTranslator.Protocol.Serializers
{
    public interface IDtoSerializer
    {
        T Deserialize<T>(string text);

        string Serialize<T>(T obj);
    }
}