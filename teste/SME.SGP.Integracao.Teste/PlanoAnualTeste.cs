using Newtonsoft.Json;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    public class PlanoAnualTeste : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public PlanoAnualTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact, Order(3)]
        public void DeveIncluirPlanoAnual()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(planoAnualDto), Encoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact, Order(4)]
        public void NaoDeveIncluirPlanoAnualEExibirMensagemErro()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();
            planoAnualDto.EscolaId = null;
            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(planoAnualDto), Encoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

            Assert.False(postResult.IsSuccessStatusCode);
            var jsonErro = postResult.Content.ReadAsStringAsync().Result;
            var retornoBase = JsonConvert.DeserializeObject<RetornoBaseDto>(jsonErro);
            Assert.Contains(retornoBase.Mensagens, c => c.Equals("A escola deve ser informada"));
        }

        private static PlanoAnualDto CriarDtoPlanoAnual()
        {
            return new PlanoAnualDto()
            {
                Ano = 1,
                Bimestre = 1,
                Descricao = "Primeiro bismestre do primeiro ano",
                EscolaId = 1,
                TurmaId = 1,
                ObjetivosAprendizagem = new List<ObjetivoAprendizagemSimplificadoDto>()
                {
                    new ObjetivoAprendizagemSimplificadoDto()
                    {
                        Id=1623,
                        IdComponenteCurricular=3
                    }
                }
            };
        }
    }
}