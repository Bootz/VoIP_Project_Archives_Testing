using System;
using System.Threading;

namespace VoipTranslator.Protocol.Commands
{
    public class Command
    {
        public Command()
        {
            PacketId = Guid.NewGuid().ToString("B");
        }

        public Command(CommandName name, string body) : this()
        {
            Name = name;
            Body = body;
        }

        public CommandName Name { get; set; }

        public string PacketId { get; set; }

        public bool IsLast { get; set; }

        public string Body { get; set; }

        public string UserId { get; set; }

        public bool Is(CommandName command)
        {
            return Name == command;
        }
    }
}
