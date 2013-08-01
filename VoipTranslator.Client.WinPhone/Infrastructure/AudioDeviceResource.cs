using System;
using Microsoft.Xna.Framework.Audio;
using VoipTranslator.Client.Core.Contracts;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class AudioDeviceResource : IAudioDeviceResource
    {
        private Microphone _microphone = null;
        private byte[] _baBuffer = null;
        private SoundEffect _soundEffect = null;

        public void StartCapture()
        {
            try
            {
                if (_microphone == null)
                {
                    _microphone = Microphone.Default;
                    _microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
                    _baBuffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
                    _microphone.BufferReady += MicrophoneBufferReady;
                    _microphone.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private void MicrophoneBufferReady(object sender, EventArgs e)
        {
            var length = _microphone.GetData(_baBuffer);
            DataPacketCaptured(this, new BinaryDataEventsArgs {Data = _baBuffer});
        }

        public void StopCapture()
        {
            try
            {
                if (_microphone != null)
                {
                    _baBuffer = new byte[0];
                    _microphone.BufferReady -= MicrophoneBufferReady;
                    _microphone.Stop();
                    _microphone = null;
                }
            }
            catch (Exception)
            {
            }
        }

        public event EventHandler<BinaryDataEventsArgs> DataPacketCaptured = delegate { };
        
        //TODO: play on gsm output device
        public void Play(byte[] packetData)
        {
            lock (_microphone)
            {
                _soundEffect = new SoundEffect(packetData, _microphone.SampleRate, AudioChannels.Mono);
                _soundEffect.Play();
                //_soundEffect.Dispose();
            }
        }
    }
}
