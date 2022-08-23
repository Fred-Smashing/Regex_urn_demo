using System.Text;

namespace Regex_urn_demo.Benchmark.Models
{
    public class BenchmarkResult
    {
        public RunResult[] BenchmarkRuns;

        public int NumberOfRuns;
        public int NumberOfResults;

        public double TotalBenchmarkTime;
        public double AverageBenchmarkTime;

        public double TotalMatchTime;
        public double AverageMatchTime;

        public double TotalParseTime;
        public double AverageParseTime;

        public BenchmarkResult(RunResult[] runs)
        {
            BenchmarkRuns = runs;
        }

        public void CalculateBenchmarkResults()
        {
            if (BenchmarkRuns.Length == 0)
            {
                return;
            }

            foreach (var run in BenchmarkRuns)
            {
                TotalBenchmarkTime += run.TotalRunTime;
                TotalMatchTime += run.TotalMatchTime;
                TotalParseTime += run.TotalParseTime;
            }

            NumberOfRuns = BenchmarkRuns.Length;

            AverageBenchmarkTime = TotalBenchmarkTime / BenchmarkRuns.Length;
            AverageMatchTime = TotalMatchTime / BenchmarkRuns.Length;
            AverageParseTime = TotalParseTime / BenchmarkRuns.Length;
        }

        public string DisplayResult(string benchmarkTitle)
        {
            var sb = new StringBuilder();

            var line = CreateLine(benchmarkTitle.Length * 2);

            sb.AppendLine(line);

            sb.Append($"{benchmarkTitle}:\n\n");
            sb.Append($"Ran the parser {NumberOfRuns} times\n\n");

            sb.Append($"Total Benchmark Time: {TotalBenchmarkTime:0.000}ms\n");
            //sb.Append($"\tHow long all runs took collectively\n");

            sb.Append($"Average Run Time: {AverageBenchmarkTime:0.000000}ms\n\n");
            //sb.Append($"\tHow long on average it took to match the entire file and parse a single urn\n\n");

            sb.Append($"Total Match Time: {TotalMatchTime:0.000}ms\n");
            //sb.Append($"\tTotal time spent matching the file accross all runs\n");

            sb.Append($"Average Match Time: {AverageMatchTime:0.000000}ms\n\n");
            //sb.Append($"\tHow long on average it took to match the entire file\n\n");

            sb.Append($"Total Parse Time: {TotalParseTime:0.000}ms\n");
            //sb.Append($"\tTotal time spent parsing all urns accross all runs\n");

            sb.Append($"Average Parse Time: {AverageParseTime:0.000000}ms\n\n");
            //sb.Append($"\tHow long on average it took to parse a single urn\n");

            sb.AppendLine(line);

            return sb.ToString(); ;
        }

        private string CreateLine(int lineLength)
        {
            var lineChar = '-';
            var sb = new StringBuilder();

            for (int i = 0; i <= lineLength; i++)
            {
                sb.Append(lineChar);
            }

            return sb.ToString();
        }
    }
}
