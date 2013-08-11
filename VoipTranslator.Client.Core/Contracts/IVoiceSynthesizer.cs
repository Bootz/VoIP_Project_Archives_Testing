using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoipTranslator.Client.Core.Contracts
{
    public interface IVoiceSynthesizer
    {
        void Synthesize(string text);
    }
}
