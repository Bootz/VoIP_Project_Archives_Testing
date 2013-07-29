using System;

namespace VoipTranslator.Client.Core.Contracts
{
    public class CallEventsArgs : EventArgs
    {
        public string Number { get; set; }
    }
}