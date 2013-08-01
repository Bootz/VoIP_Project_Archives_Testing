using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace VoipTranslator.Protocol.Serializers.Builtin
{
    public class DcDtoSerializer : IDtoSerializer
    {
        public T Deserialize<T>(string text)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }

        public string Serialize<T>(T obj)
        {
            using (var memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, obj);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}