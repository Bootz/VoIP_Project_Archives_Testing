
namespace VoipTranslator.Client.Core.Common
{
    public interface IKeyValueStorage
    {
        T GetValue<T>(string key, T defaultValue = default(T));
        void SetValue(string key, object value);
    }
}
