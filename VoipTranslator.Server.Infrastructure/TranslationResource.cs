using System;
using System.IO;
using System.Speech.Recognition;
using VoipTranslator.Server.Application.Contracts;

namespace VoipTranslator.Server.Infrastructure
{
    public class TranslationResource : ITranslationResource
    {
        private object _syncObject = new object();
        private SpeechRecognitionEngine _recognizer;

        public TranslationResource()
        {
            _recognizer = new SpeechRecognitionEngine();
            Grammar dictationGrammar = new DictationGrammar();
            _recognizer.LoadGrammar(dictationGrammar);
        }

        public void AppendRawData(byte[] data, Action<byte[]> callback)
        {
            var stream = new MemoryStream(data);
            _recognizer.SetInputToAudioStream(stream, new System.Speech.AudioFormat.SpeechAudioFormatInfo(128000, System.Speech.AudioFormat.AudioBitsPerSample.Sixteen, System.Speech.AudioFormat.AudioChannel.Mono));
            var result = _recognizer.Recognize();
            var text = result.Text;

            //callback(data);
            //AppendAllBytes("D:\\raw.data", data);
        }

        public void AppendAllBytes(string path, byte[] bytes)
        {
            lock (_syncObject)
            {
                using (var stream = new FileStream(path, FileMode.Append))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
