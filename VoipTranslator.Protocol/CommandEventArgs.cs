using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoipTranslator.Protocol
{
    public class CommandEventArgs : EventArgs
    {
        public Command Command { get; set; }
    }
}
