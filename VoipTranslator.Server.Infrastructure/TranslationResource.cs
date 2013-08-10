using System;
using System.Speech.Recognition;
using VoipTranslator.Server.Application.Contracts;

namespace VoipTranslator.Server.Infrastructure
{
    public class TranslationResource : ITranslationResource
    {
        private SpeechRecognitionEngine _recognizer;

        public TranslationResource()
        {
            _recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            _recognizer.LoadGrammar(dictationGrammar);
        }

        public void AppendRawData(byte[] data, Action<byte[]> callback)
        {
            callback(data);
        }
    }
}
