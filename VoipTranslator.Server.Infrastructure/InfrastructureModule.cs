using Autofac;
using VoipTranslator.Infrastructure.Logging;
using VoipTranslator.Protocol.Contracts;
using VoipTranslator.Server.Application.Contracts;
using VoipTranslator.Server.Domain.Entities.User;
using VoipTranslator.Server.Domain.Seedwork;
using VoipTranslator.Server.Infrastructure.Network;
using VoipTranslator.Server.Infrastructure.Persistence;

namespace VoipTranslator.Server.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransportResource>().As<ICommandsTransportResource>().SingleInstance();
            builder.RegisterType<PushNotificationResource>().As<IPushNotificationResource>().SingleInstance();
            builder.RegisterType<InMemoryUserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<ConsoleLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<EmptyUserIdProvider>().As<IUserIdProvider>().SingleInstance();
            builder.RegisterType<TransactionFactoryStub>().As<ITransactionFactory>().SingleInstance();
            builder.RegisterType<TranslationResource>().As<ITranslationResource>().SingleInstance();

            base.Load(builder);
        }
    }
}
