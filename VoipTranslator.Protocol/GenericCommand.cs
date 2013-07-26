using System.Threading;

namespace VoipTranslator.Protocol
{
    public class GenericCommand<T> : Command
    {
        public GenericCommand()
        {
        }

        public GenericCommand(CommandName name, T body) : base(name)
        {
            Name = name;
            Body = body;
            SetIdentity();
        }

        public T Body { get; set; }
    }
}
