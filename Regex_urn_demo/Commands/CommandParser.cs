using Regex_urn_demo.Benchmark;
using Regex_urn_demo.Benchmark.Models;
using Regex_urn_demo.UrnValidation;
using Regex_urn_demo.UrnValidation.Abstractions;
using Regex_urn_demo.UrnValidation.Models;

namespace Regex_urn_demo.Commands
{
    public static class CommandParser
    {
        private const string validateCmd = "validate";
        private const string parseCmd = "parse-file";
        private const string benchmarkCmd = "benchmark";

        private static readonly string defaultFilePath = @"Data\urn-data.txt";
        public static bool IsRunning { get; private set; } = false;

        private static readonly IUrnParser regexParser = new RegexUrnParser();
        private static readonly IUrnParser customUrnParser = new CustomUrnParser();

        public static void Parse(string input)
        {
            IsRunning = true;
            Console.WriteLine("Running Command");

            var splits = input.Split(' ');
            var cmd = splits[0].Trim();
            var args = splits[1..splits.Length];

            RunCommand(cmd, args);
        }

        private static void RunCommand(string command, string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments passed into the command");
            }

            switch (command)
            {
                case validateCmd:
                    ValidateCommand(args[1], args[0]);
                    break;
                case parseCmd:
                    ParseFileCommand(args[1], args[0]);
                    break;
                case benchmarkCmd:
                    BenchmarkCommand(args[0]);
                    break;
                default:
                    Complete();
                    break;
            }
        }

        private static void ValidateCommand(string parserArg, string urn)
        {
            var parser = GetParser(parserArg);
            var data = parser.GetUrnData(urn);
            WriteUrnDataToConsole(data);
            Complete();
        }

        private static void ParseFileCommand(string parserArg, string filePath)
        {
            string path;
            switch (filePath)
            {
                case "default":
                    path = defaultFilePath;
                    break;
                default:
                    path = filePath;
                    break;
            }

            try
            {
                var fileText = File.ReadAllText(path);
                var parser = GetParser(parserArg);
                var data = parser.GetUrnData(fileText);
                WriteUrnDataToConsole(data);
            }
            catch (IOException)
            {
                throw new IOException($"{path} was not a recognised file path");
            }

            Complete();
        }

        private static void BenchmarkCommand(string arg)
        {
            try
            {
                var numRuns = int.Parse(arg);

                var regexResultSingle = BenchmarkRunner.ParseSingelUrnBenchmark(new RegexUrnParser(), numRuns);
                var customResultSingle = BenchmarkRunner.ParseSingelUrnBenchmark(new CustomUrnParser(), numRuns);

                WriteBenchmarkToConsole(regexResultSingle, "Regex Parser Single Urn Benchmark Result");
                WriteBenchmarkToConsole(customResultSingle, "Custom Parser Single Urn Benchmark Result");

                var regexResultFile = BenchmarkRunner.ParseFileBenchmark(new RegexUrnParser(), numRuns);
                var customResultFile = BenchmarkRunner.ParseFileBenchmark(new CustomUrnParser(), numRuns);

                WriteBenchmarkToConsole(regexResultFile, "Regex Parser File Benchmark Result");
                WriteBenchmarkToConsole(customResultFile, "Custom Parser File Benchmark Result");
            }
            catch (FormatException)
            {
                throw new FormatException($"{arg} is not a valid integer number");
            }

            Console.WriteLine("Benchmark Complete");
            Complete();
        }

        private static IUrnParser GetParser(string parserArg)
        {
            switch (parserArg)
            {
                case "-r":
                    return regexParser;
                case "-c":
                    return customUrnParser;
                default:
                    throw new ArgumentException($"{parserArg} is not a recognised argument");
            }
        }

        private static void WriteUrnDataToConsole(UrnDataObject[] dataObjects)
        {
            Console.WriteLine("Results:\n");
            for (var i = 0; i <dataObjects.Length; i++)
            {
                Console.WriteLine(dataObjects[i].ToString());

                if (i < dataObjects.Length - 1)
                {
                    Console.WriteLine();
                }
            }
        }

        private static void WriteBenchmarkToConsole(BenchmarkResult benchmarkResult, string title)
        {
            Console.WriteLine(benchmarkResult.DisplayResult(title));
        }

        private static void Complete()
        {
            Console.WriteLine($"\nCommand Completed\n");
            IsRunning = false;
        }
    }
}
