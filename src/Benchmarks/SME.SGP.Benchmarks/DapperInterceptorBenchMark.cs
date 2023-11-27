using BenchmarkDotNet.Attributes;
using SME.SGP.Dados;
using SME.SGP.Dados.Contexto;
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
            return default;
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }


    }
}