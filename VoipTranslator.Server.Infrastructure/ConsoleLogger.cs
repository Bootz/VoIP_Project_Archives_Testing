using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Server.Logging;

namespace VoipTranslator.Server.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        private readonly string _type;

        public ConsoleLogger(string type)
        {
            _type = type;
        }

        public void Trace(string format, params object[] args)
        {
            const ConsoleColor color = ConsoleColor.DarkGray;
            Append(color, string.Format(format, args));
        }

        public void Debug(string format, params object[] args)
        {
            const ConsoleColor color = ConsoleColor.Gray;
            Append(color, string.Format(format, args));
        }

        public void Info(string format, params object[] args)
        {
            const ConsoleColor color = ConsoleColor.White;
            Append(color, string.Format(format, args));
        }

        public void Warn(string format, params object[] args)
        {
            const ConsoleColor color = ConsoleColor.Yellow;
            Append(color, string.Format(format, args));
        }

        public void Error(string format, params object[] args)
        {
            const ConsoleColor color = ConsoleColor.Magenta;
            Append(color, string.Format(format, args));
        }

        public void Exception(Exception exc)
        {
            const ConsoleColor color = ConsoleColor.Red;
            Append(color, GetExceptionDetails(exc));
        }

        private string GetExceptionDetails(Exception exc)
        {
            //TODO: inner exceptions
            return string.Format("Exception: {0} ({1}). StackTrace:\n{2}", exc.GetType().Name, exc.Message, exc.StackTrace);
        }

        public void Exception(Exception exc, string format, params object[] args)
        {
            ConsoleColor color = ConsoleColor.Red;
            Append(color, GetExceptionDetails(exc) + ":\n" + string.Format(format, args));
        }

        private static readonly object SyncObj = new object();

        private void Append(ConsoleColor color, string str)
        {
            lock (SyncObj)
            {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(_type + " | " +str + Environment.NewLine);
                Console.ForegroundColor = prevColor;
            }
        }
    }
}
