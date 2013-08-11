using System;
using System.IO;
using VoipTranslator.Infrastructure;
using VoipTranslator.Server.Application.Contracts;

namespace VoipTranslator.Server.Application.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Run();
            Console.WriteLine("Bootstrapper run.");
            string line = string.Empty;

            ServiceLocator.Resolve<ITranslationResource>().AppendRawData(File.ReadAllBytes("D:\\raw.data"), b => { });

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
