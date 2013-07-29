using System.Collections.Generic;
using Autofac;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Server;
using VoipTranslator.Server.Infrastructure;

namespace Voip.Server.ConsoleHost
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

            var modules = new List<Module>
                {
                    new InfrastructureModule(),
                    new CoreModule(),
                };

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            var container = builder.Build();
            ServiceLocator.Init(container);
            container.Resolve<AccountService>();
            container.Resolve<VoiceManager>();

            container.Resolve<CommandBuilder>().Create(CommandName.Dial, 3);
            var cmd = container.Resolve<ICommandSerializer>().Serialize(new VoipTranslator.Protocol.Command { Body = "ddd "});
            var c = container.Resolve<ICommandSerializer>().Deserialize(cmd);
        }
    }
}
