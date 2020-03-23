using BenchmarkDotNet.Attributes;

namespace SME.SGP.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class RedisGZipBenchMark
    {
        //private readonly IRepositorioCache repositorioCache;

        //public RedisGZipBenchMark()
        //{
        //    //IServiceCollection serviceCollection = new ServiceCollection();
        //    //ConfigureServices(serviceCollection);

        //    //var services = serviceCollection.BuildServiceProvider();

        //    //repositorioCache = new RepositorioCache(services.GetService<IDistributedCache>(), services.GetService<IServicoLog>());
        //}

        [Benchmark]
        public void CacheComGZip()
        {
            // repositorioCache.Obter("teste", true);
        }

        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    //services.AddDistributedRedisCache(options =>
        //    //{
        //    //    options.Configuration = "localhost";
        //    //    options.InstanceName = "SGP-";
        //    //});

        //}
    }
}