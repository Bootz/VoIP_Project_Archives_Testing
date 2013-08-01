using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoipTranslator.Protocol
{
    public interface IUserIdProvider
    {
        string UserId { get; set; }
    }
}
