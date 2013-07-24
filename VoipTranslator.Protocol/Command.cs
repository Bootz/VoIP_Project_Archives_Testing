namespace VoipTranslator.Protocol
{
    public class Command
    {
        public Command()
        {
        }

        public Command(CommandName name, string body)
        {
            Name = name;
            Body = body;
        }

        public CommandName Name { get; set; }

        public string Body { get; set; }

        public bool Is(CommandName command)
        {
            return Name == command;
        }
    }
}
