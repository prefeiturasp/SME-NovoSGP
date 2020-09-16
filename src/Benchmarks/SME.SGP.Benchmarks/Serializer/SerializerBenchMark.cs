using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    public class SerializerBenchMark
    {
        private int n = 10;
        private string listaAlunos;

        public SerializerBenchMark()
        {
            listaAlunos = Resource.Json.AlunosPorTurma;
        }

        [Benchmark]
        public void NewtonSoft()
        {
            for (int i = 0; i < n; i++)
            {
                _ = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

        [Benchmark]
        public void SystemTextJson()
        {
            for (int i = 0; i < n; i++)
            {
                _ = System.Text.Json.JsonSerializer.Deserialize<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

        [Benchmark]
        public void SGPJsonSerializer()
        {
            for (int i = 0; i < n; i++)
            {
                _ = SgpJsonSerializer.Deserialize<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

    }
}
