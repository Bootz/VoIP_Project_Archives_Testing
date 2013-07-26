using System;

namespace VoipTranslator.Protocol.Dto
{
    public class PacketEventArgs : EventArgs
    {
        public string Data { get; set; }
    }
}
