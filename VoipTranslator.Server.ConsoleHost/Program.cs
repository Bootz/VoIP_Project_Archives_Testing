using System;

namespace Voip.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Run();
            string line = string.Empty;
            while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
            {
                line = Console.ReadLine();
            }
        }
    }
}
