using System.Xml.Schema;
using System.Xml.Serialization;

namespace VoipTranslator.Protocol
{
    [XmlType(AnonymousType = true, Namespace = "WPNotification")]
    [XmlRoot(Namespace = "WPNotification", IsNullable = false)]
    public class IncomingCallInfo
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
    }
}
