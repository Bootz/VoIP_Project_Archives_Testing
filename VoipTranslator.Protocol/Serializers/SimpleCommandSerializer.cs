using System;

namespace VoipTranslator.Protocol.Serializers
{
    public class SimpleCommandSerializer : ICommandSerializer
    {
        public const string Separator = " ";

        public Command Deserialize(string text)
        {
            int spaceIndex = text.IndexOf(Separator, StringComparison.Ordinal);
            if (spaceIndex < 0)
                throw new ArgumentException("text is empty");

            string commandName = text.Substring(0, spaceIndex);
            CommandName cmd;

            if (!Enum.TryParse(commandName, out cmd))
            {
                throw new ArgumentException("text is invalid: " + text);
            }
            return new Command(cmd, text.Substring(spaceIndex + 1));
        }

        public string Serialize(Command cmd)
        {
            return cmd.Name + Separator + cmd.Body;
        }
    }
}
