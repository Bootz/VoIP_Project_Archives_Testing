using System;

namespace VoipTranslator.Server.Application.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Run();
            Console.WriteLine("Bootstrapper run.");
            string line = string.Empty;
           
            while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                line = Console.ReadLine();
                //if (line == "pushtest")
                //{
                //    var user = ServiceLocator.Resolve<IUsersRepository>().GetLastUser();
                //    if (user == null)
                //    {
                //        Console.WriteLine("user wasn't found");
                //    }
                //    else
                //    {
                //        ServiceLocator.Resolve<IPushSender>().SendVoipPush(user.PushUri, user.Number, user.Number);
                //    }
                //}
            }
        }
    }
}
