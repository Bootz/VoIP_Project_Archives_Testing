using System;

namespace VoipTranslator.Protocol.Dto
{
    public class PacketEventArgs : EventArgs
    {
        public string Data { get; set; }
        public string RemotePeerAddress { get; set; }
        public string RemotePeerPort { get; set; }
    }
}
