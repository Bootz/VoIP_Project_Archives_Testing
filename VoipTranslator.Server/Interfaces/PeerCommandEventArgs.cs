﻿using VoipTranslator.Protocol;

namespace VoipTranslator.Server.Interfaces
{
    public class PeerCommandEventArgs : PacketEventArgs
    {
        public IRemotePeer Peer { get; set; }
    }
}