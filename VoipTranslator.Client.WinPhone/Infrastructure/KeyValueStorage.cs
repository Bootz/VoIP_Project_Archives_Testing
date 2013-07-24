using System.IO.IsolatedStorage;
using VoipTranslator.Client.Core.Common;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class KeyValueStorage : IKeyValueStorage
    {
        private readonly IsolatedStorageSettings _storage = IsolatedStorageSettings.ApplicationSettings;
        private static readonly object SyncObj = new object();

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            lock (SyncObj)
            {
                T value;
                return _storage.TryGetValue(key, out value) ? value : defaultValue;
            }
        }

        public void SetValue(string key, object value)
        {
            lock (SyncObj)
            {
                try
                {
                    if (value == null && _storage.Contains(key))
                    {
                        _storage.Remove(key);
                        _storage.Save();
                    }
                    else if (value != null)
                    {
                        _storage[key] = value;
                        _storage.Save();
                    }
                }
                catch (System.Exception exc)
                {
                    //Logger.Exception(exc, "Trying to save key:{0} value:{1}", key, value);
                    throw;
                }
            }
        }
    }
}
