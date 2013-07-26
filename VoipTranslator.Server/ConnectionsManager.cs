using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server
{
    public class ConnectionsManager
    {
        private readonly ITransportResource _transportManager;

        public ConnectionsManager(ITransportResource transportManager)
        {
            _transportManager = transportManager;
        }
    }
}
