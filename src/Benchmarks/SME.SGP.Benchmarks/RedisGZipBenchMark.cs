using BenchmarkDotNet.Attributes;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using System;

namespace SME.SGP.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class RedisGZipBenchMark
    {
        private readonly Lazy<ConnectionMultiplexer> conexao;

        public RedisGZipBenchMark()
        {
            conexao = ObterConexao();
        }

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
            //var conexao = ObterConexao();
            try
            {
                var valorParaGravar = Convert.ToBase64String(UtilGZip.Comprimir("12345678910"));

                conexao.Value.GetDatabase().StringSet("teste-benchmark", valorParaGravar, TimeSpan.FromMinutes(10));
                var valorReortono = UtilGZip.Descomprimir(Convert.FromBase64String(conexao.Value.GetDatabase().StringGet("teste-benchmark")));

                conexao.Value.GetDatabase().KeyDelete("teste-benchmark");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //   Dispose(conexao);
            }
        }

        [Benchmark]
        public void CacheSemGZip()
        {
            //var conexao = ObterConexao();
            try
            {
                var valorParaGravar = "12345678910";

                conexao.Value.GetDatabase().StringSet("teste-benchmark", valorParaGravar, TimeSpan.FromMinutes(10));
                conexao.Value.GetDatabase().StringGet("teste-benchmark");

                conexao.Value.GetDatabase().KeyDelete("teste-benchmark");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //   Dispose(conexao);
            }
        }

        private void Dispose(Lazy<ConnectionMultiplexer> conexao)
        {
            if (conexao != null && conexao.IsValueCreated)
            {
                conexao.Value.Close();
                conexao.Value.Dispose();
            }
        }

        private Lazy<ConnectionMultiplexer> ObterConexao()
        {
            return new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect($"localhost,allowAdmin=true,syncTimeout=1000,connectTimeout=1000"); });
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