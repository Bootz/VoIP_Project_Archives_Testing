using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoipTranslator.Client.Core.Contracts
{
    public interface IAudioDeviceResource
    {
        void StartCapture();

        void StopCapture();

        event EventHandler<BinaryDataEventsArgs> DataPacketCaptured;

        void Play(byte[] packetData);
    }
}
