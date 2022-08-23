using Regex_urn_demo.Commands;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Regex_urn_demo
{
    internal class Program
    {
        private static bool _appRunning = true;

        public static void SetOptions()
        {
        }

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Input urn command to proceed\n");
                var input = Console.ReadLine();

                CommandParser.Parse(input!);

                while (CommandParser.IsRunning)
                {
                    Console.Write(".");
                }
            }
            while (_appRunning);
        }
    }
}