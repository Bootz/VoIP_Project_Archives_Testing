using System;
using System.Collections.Generic;
using System.Linq;

namespace Voip.Server.ConsoleHost
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
            }
        }
    }
}
