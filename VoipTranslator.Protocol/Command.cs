using System.Threading;

namespace VoipTranslator.Protocol
{
    public class Command
    {
        private static long _lastId = 0;

        public Command()
        {
            Interlocked.Increment(ref _lastId);
            PacketId = _lastId;
        }

        public Command(CommandName name, object body) : this()
        {
            Name = name;
            Body = body;
        }

        public CommandName Name { get; set; }

        public long PacketId { get; set; }

        public bool IsLast { get; set; }

        public object Body { get; set; }

        public bool Is(CommandName command)
        {
            return Name == command;
        }
    }
}
