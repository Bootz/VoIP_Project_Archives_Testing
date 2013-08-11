using System;
using System.Linq;
using System.Threading;
using Windows.Phone.Speech.Synthesis;
using VoipTranslator.Client.Core.Contracts;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class VoiceSynthesizer : IVoiceSynthesizer
    {
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private SpeechSynthesizer _speechSynthesizer;

        public VoiceSynthesizer()
        {
            _speechSynthesizer = new SpeechSynthesizer();
            var all = InstalledVoices.All.ToArray();
        }

        public async void Synthesize(string text)
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                await _speechSynthesizer.SpeakTextAsync(text);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
