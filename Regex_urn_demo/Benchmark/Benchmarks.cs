using BenchmarkDotNet.Attributes;
using Regex_urn_demo.UrnValidation;
using Regex_urn_demo.UrnValidation.Models;
using System.Security.AccessControl;

namespace Regex_urn_demo.Benchmark
{
    [SimpleJob(id: "Parser Benchmarks")]
    public class Benchmarks
    {
        public struct FilePaths
        {
            public static readonly string singleUrnFilePath = @"Data\benchmark-data.txt";
            public static readonly string urnInTextFilePath = @"Data\urn-data.txt";
        }

        private readonly RegexUrnParser RegexParser;
        private readonly CustomUrnParser CustomParser;

        private readonly string[] allLines;
        private readonly string fileText;

        private string? singleLine;
        private int lastLine = -1;

        public Benchmarks()
        {

            allLines = File.ReadLines(FilePaths.singleUrnFilePath).ToArray();
            fileText = File.ReadAllText(FilePaths.urnInTextFilePath);

            RegexParser = new();
            CustomParser = new();
        }

        [GlobalSetup]
        public void Setup()
        {
            singleLine = GetRandomLine(allLines);
        }

        private string GetRandomLine(string[] lines)
        {
            int num;
            do
            {
                num = new Random().Next(0, lines.Count());
            } while (num == lastLine);
            lastLine = num;
            return lines[num];
        }

        [Benchmark]
        public UrnDataObject[] SinlgeUrn_RegexParser()
        {
            return RegexParser.GetUrnData(singleLine);
        }

        [Benchmark]
        public UrnDataObject[] File_RegegexParser()
        {
            return RegexParser.GetUrnData(fileText);
        }

        [Benchmark]
        public UrnDataObject[] SingleUrn_CustomParser()
        {
            return CustomParser.GetUrnData(singleLine);
        }
        [Benchmark]
        public UrnDataObject[] File_CustomParser()
        {
            return CustomParser.GetUrnData(fileText);
        }
    }
}
