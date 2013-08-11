using System;
using System.Threading.Tasks;
using Windows.Phone.Speech.Recognition;
using Autofac;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.Core.Compasition;
using VoipTranslator.Client.WinPhone.Infrastructure;
using VoipTranslator.Infrastructure;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.WinPhone
{
    public static class Bootstrapper
    {
        private static bool _wasRun;

        public static async void Run()
        {
            if (_wasRun)
                return;
            _wasRun = true;

            var builder = new ContainerBuilder();

            var modules = new LayerModule[]
                {
                    new CoreModule(),
                    new ApplicationModule(),
                };

            modules.ForEach(builder.RegisterModule);

            var container = builder.Build();
            ServiceLocator.Init(container);
            modules.ForEach(m => m.OnPostContainerBuild(container));
            //new AudioDeviceResource().StartCapture();
            var s = new VoiceSynthesizer();
            string[] texts =
            {
                "Привет мир",
                "как дела",
                "Hello world",
                "How are you?",
                "test",
                "give me that thing please",
                "thats why I here",
                "translation test",
            };
            foreach (var text in texts)
            {
                await Task.Delay(500);
                s.Synthesize(text);
            }
        }
    }
}
