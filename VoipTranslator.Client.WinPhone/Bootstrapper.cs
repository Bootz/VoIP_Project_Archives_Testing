using Autofac;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.Core.Compasition;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.WinPhone
{
    public static class Bootstrapper
    {
        private static bool _wasRun;

        public static void Run()
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
        }
    }
}
