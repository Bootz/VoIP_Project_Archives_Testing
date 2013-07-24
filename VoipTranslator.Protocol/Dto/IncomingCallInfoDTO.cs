using System.Xml.Schema;
using System.Xml.Serialization;

namespace VoipTranslator.Protocol.Dto
{
    [XmlType(AnonymousType = true, Namespace = "WPNotification")]
    [XmlRoot(Namespace = "WPNotification", IsNullable = false)]
    public class IncomingCallInfoDto
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
    }
}
