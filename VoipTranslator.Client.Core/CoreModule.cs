using Autofac;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.Core.Compasition;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Protocol.Contracts;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Protocol.Serializers.Builtin;

namespace VoipTranslator.Client.Core
{
    public class CoreModule : LayerModule
    {
        protected override void OnMap(ContainerBuilder builder)
        {
            builder.RegisterType<AccountManager>().AsSelf().SingleInstance();
            builder.RegisterType<ApplicationManager>().AsSelf().SingleInstance();
            builder.RegisterType<TransportManager>().AsSelf().SingleInstance();
            builder.RegisterType<CallsManager>().AsSelf().SingleInstance();
            builder.RegisterType<CommandBuilder>().AsSelf().SingleInstance();
            builder.RegisterType<CommandSerializer>().As<ICommandSerializer>().SingleInstance();
            builder.RegisterType<XmlDtoSerializer>().As<IDtoSerializer>().SingleInstance();
            builder.RegisterType<UserIdProvider>().As<IUserIdProvider>().SingleInstance();

            base.OnMap(builder);
        }
    }
}
