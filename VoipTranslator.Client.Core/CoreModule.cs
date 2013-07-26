using Autofac;
using VoipTranslator.Client.Core.Compasition;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Serializers;

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
            builder.RegisterType<SimpleCommandSerializer>().As<ICommandSerializer>().SingleInstance();
            builder.RegisterType<XmlDtoSerializer>().As<IDtoSerializer>().SingleInstance();


            base.OnMap(builder);
        }
    }
}
