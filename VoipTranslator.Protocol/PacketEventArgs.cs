using System;

namespace VoipTranslator.Protocol
{
    public class PacketEventArgs : EventArgs
    {
        public string Data { get; set; }
    }
}
