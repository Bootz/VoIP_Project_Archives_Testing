using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace VoipTranslator.Protocol.Serializers
{
    public class XmlCommandSerializer : ICommandSerializer
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Command));

        public Command Deserialize(string text)
        {
            Command result;
            using (TextReader reader = new StringReader(text))
            {
                result = Serializer.Deserialize(reader) as Command;
            }
            return result;
        }

        public string Serialize(Command cmd)
        {
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                Serializer.Serialize(writer, cmd);
            }
            return sb.ToString();
        }
    }
}