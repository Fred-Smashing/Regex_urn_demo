using Regex_urn_demo.Benchmark.Models;
using Regex_urn_demo.UrnValidation.Abstractions;

namespace Regex_urn_demo.Benchmark
{
    public static class BenchmarkRunner
    {
        private static readonly string singleUrnFilePath = @"Data\benchmark-data.txt";
        private static readonly string urnInTextFilePath = @"Data\urn-data.txt";

        public static BenchmarkResult ParseSingelUrnBenchmark(IUrnParser parser, int numberOfRuns)
        {
            var lines = File.ReadLines(singleUrnFilePath).ToArray();

            List<RunResult> runResults = new();

            var lineIndex = 0;
            for (int i = 0; i < numberOfRuns; i++)
            {
                var urnData = parser.GetUrnData(lines[lineIndex]);

                if (urnData is not null && urnData.Length > 0)
                {
                    var runResult = urnData[0].runResult;
                    runResult.RunNumber = i;

                    runResults.Add(runResult);
                }

                lineIndex++;
                if (lineIndex >= lines.Length)
                {
                    lineIndex = 0;
                }
            }

            return CreateBenchmarkResult(runResults);
        }

        public static BenchmarkResult ParseFileBenchmark(IUrnParser parser, int numberOfRuns)
        {
            var fileText = File.ReadAllText(urnInTextFilePath);

            List<RunResult> runResults = new();

            for (int i = 0; i < numberOfRuns; i++)
            {
                var urnData = parser.GetUrnData(fileText);

                if (urnData is not null)
                {
                    double totalMatchTime = 0;
                    double totalParseTime = 0;
                    foreach (var data in urnData)
                    {
                        totalMatchTime = data.runResult.TotalMatchTime;
                        totalParseTime += data.runResult.TotalParseTime;
                    }

                    var result = BenchmarkRunUtils.CreateRunResult(totalMatchTime, totalParseTime);

                    runResults.Add(result);
                }
            }

            return CreateBenchmarkResult(runResults);
        }

        private static BenchmarkResult CreateBenchmarkResult(List<RunResult> runs)
        {
            var result = new BenchmarkResult(runs.ToArray());
            result.CalculateBenchmarkResults();
            return result;
        }
    }
}
