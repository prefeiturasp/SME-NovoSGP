using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dados;
using System;

namespace SME.SGP.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class DapperInterceptorBenchMark
    {


        private SgpContext conexao;

        private SgpContext ObterConexao()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

            return new SgpContext(configuration, new ContextoBenchmark());
        }
        [GlobalSetup]
        public void Setup()
        {
            conexao = ObterConexao();
        }
        [Benchmark]
        public void ConsultaBasica()
        {
            var query = @"select *
                 from usuario where id = 1";


            try
            {
                var retorno = conexao.Query(query);
                //var repositorioUsuario = new RepositorioUsuario(conexao);
                //repositorioUsuario.ObterPorId(1);
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


    }
}