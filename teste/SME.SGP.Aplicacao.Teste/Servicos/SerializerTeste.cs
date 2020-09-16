using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    public class SerializerTeste
    {
        private int n = 1000;
        private string listaAlunos;

        public SerializerTeste()
        {
            listaAlunos = Resource.ResourceJson.AlunosPorTurma;
        }

        [Fact]
        public async Task Tempo_NewtonSoft()
        {
            for (int i = 0; i < n; i++)
            {
                _ = JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

        [Fact]
        public async Task Tempo_System_Text_Json()
        {
            for (int i = 0; i < n; i++)
            {
                _ = System.Text.Json.JsonSerializer.Deserialize<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

        [Fact]
        public async Task Tempo_SGPJsonSerializer()
        {
            for (int i = 0; i < n; i++)
            {
                _ = SgpJsonSerializer.Deserialize<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

        [Fact]
        public async Task Tempo_SGPJsonSerializerAsync()
        {
            for (int i = 0; i < n; i++)
            {
                _ = await SgpJsonSerializer.DeserializeAsync<List<AlunoPorTurmaResposta>>(listaAlunos);
            }
        }

    }
}
