using Regex_urn_demo.Benchmark.Models;
using System.Diagnostics;

namespace Regex_urn_demo.Benchmark
{
    public static class BenchmarkRunUtils
    {
        public static Stopwatch StartNewStopwatch()
        {
            var sw = new Stopwatch();
            sw.Start();
            return sw;
        }

        public static void RestartStopwatch(Stopwatch stopwatch)
        {
            stopwatch.Restart();
        }

        public static void StopStopwatch(Stopwatch stopwatch)
        {
            stopwatch.Stop();
        }

        public static double GetElapsedMs(Stopwatch stopwatch)
        {
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public static RunResult CreateRunResult(double matchTime, double parseTime)
        {
            return new RunResult()
            {
                TotalRunTime = matchTime + parseTime,
                TotalMatchTime = matchTime,
                TotalParseTime = parseTime
            };
        }
    }
}
