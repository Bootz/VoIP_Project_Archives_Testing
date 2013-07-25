using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace VoipTranslator.Protocol.Serializers
{
    public class XmlCommandSerializer<T> where T : class
    {
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public T Deserialize(string text)
        {
            T result;
            using (TextReader reader = new StringReader(text))
            {
                result = _serializer.Deserialize(reader) as T;
            }
            return result;
        }

        public string Serialize(T cmd)
        {
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                _serializer.Serialize(writer, cmd);
            }
            return sb.ToString();
        }
    }
}