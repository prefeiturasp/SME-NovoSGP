using BenchmarkDotNet.Running;

namespace SME.SGP.Benchmarks
{
    public class Program
    {
        protected Program() { }

        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<RedisGZipBenchMark>();
        }
    }
}