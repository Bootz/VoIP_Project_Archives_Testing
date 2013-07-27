using Autofac;
using VoipTranslator.Server.Interfaces;
using VoipTranslator.Server.Logging;

namespace VoipTranslator.Server.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransportResource>().As<ITransportResource>().SingleInstance();
            builder.RegisterType<PushSender>().As<IPushSender>().SingleInstance();
            builder.RegisterType<UsersRepository>().As<IUsersRepository>().SingleInstance();
            builder.RegisterType<ConsoleLogger>().As<ILogger>().SingleInstance();

            base.Load(builder);
        }
    }
}
