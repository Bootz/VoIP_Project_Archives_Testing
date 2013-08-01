using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VoipTranslator.Server;
using VoipTranslator.Server.Interfaces;

namespace Voip.Server.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Run();
            Console.WriteLine("Bootstrapper run.");
            string line = string.Empty;

            CultureInfo turkish = CultureInfo.CreateSpecificCulture("tr");
            //Thread.CurrentThread.CurrentCulture = turkish;

            // In Turkey, "i" does odd things
            string lower = "i";
            string upper = "I";
            var a = lower == upper;
            
            while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                line = Console.ReadLine();
                if (line == "pushtest")
                {
                    var user = ServiceLocator.Resolve<IUsersRepository>().GetLastUser();
                    if (user == null)
                    {
                        Console.WriteLine("user wasn't found");
                    }
                    else
                    {
                        ServiceLocator.Resolve<IPushSender>().SendVoipPush(user.PushUri, user.Number, user.Number);
                    }
                }
            }
        }
    }
}
