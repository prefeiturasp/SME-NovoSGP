using Newtonsoft.Json;
using SME.SGP.Dto;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    public class PlanoAnualTeste : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public PlanoAnualTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public void DeveIncluirPlanoAnual()
        {
            fixture._clientApi.DefaultRequestHeaders.Clear();
            PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(planoAnualDto), Encoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);
        }

        [Fact]
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
                //IdsDisciplinas = new List<long> { 1, 2, 3 },
                //ObjetivosAprendizagem = new List<long> { 4, 5, 6 }
            };
        }
    }
}