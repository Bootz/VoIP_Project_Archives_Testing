using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace VoipTranslator.Protocol.Serializers
{
    public class XmlDtoSerializer : IDtoSerializer
    {
        public T Deserialize<T>(string text)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(text))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }

        public string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, obj);
            }
            return sb.ToString();
        }
    }
}