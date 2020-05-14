using BenchmarkDotNet.Running;

namespace SME.SGP.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<RedisGZipBenchMark>();
        }
    }
}