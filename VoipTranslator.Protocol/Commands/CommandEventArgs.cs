using System;

namespace VoipTranslator.Protocol.Commands
{
    public class CommandEventArgs : EventArgs
    {
        public Command Command { get; set; }
    }
}
