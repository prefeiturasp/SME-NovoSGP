using Newtonsoft.Json;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class PlanoAnualTeste
    {
        private readonly TestServerFixture fixture;

        public PlanoAnualTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact, Order(3)]
        public void DeveIncluirPlanoAnual()
        {
            try
            {
                fixture._clientApi.DefaultRequestHeaders.Clear();
                PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();

                var jsonParaPost = new StringContent(JsonConvert.SerializeObject(planoAnualDto), Encoding.UTF8, "application/json");

                var postResult = fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

                Assert.True(postResult.IsSuccessStatusCode);
                var filtro = new FiltroPlanoAnualDto()
                {
                    Ano = 2019,
                    Bimestre = 1,
                    EscolaId = "095346",
                    TurmaId = 2008187
                };
                var filtroPlanoAnual = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");

                var planoAnualCompletoResponse = fixture._clientApi.PostAsync("api/v1/planos/anual/obter", filtroPlanoAnual).Result;
                if (planoAnualCompletoResponse.IsSuccessStatusCode)
                {
                    var planoAnualCompleto = JsonConvert.DeserializeObject<PlanoCicloCompletoDto>(planoAnualCompletoResponse.Content.ReadAsStringAsync().Result);
                    Assert.Contains(planoAnualDto.Bimestres, c => c.Descricao == planoAnualCompleto.Descricao);

                    var planoAnualExistenteResponse = fixture._clientApi.PostAsync("api/v1/planos/anual/validar-existente", filtroPlanoAnual).Result;
                    Assert.True(bool.Parse(planoAnualExistenteResponse.Content.ReadAsStringAsync().Result));
                }
                else
                {
                    var erro = postResult.Content.ReadAsStringAsync().Result;
                    Assert.True(false, erro);
                }
            }
            catch (System.Exception ex)
            {
                Assert.True(false, $"Erro nas integrações: {ex.Message}");
            }
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
                AnoLetivo = 2019,
                EscolaId = "095346",
                TurmaId = 2008187,
                Bimestres = new List<BimestrePlanoAnualDto>
                {
                    new BimestrePlanoAnualDto
                    {
                        Bimestre = 1,
                        Descricao = "Primeiro bismestre do primeiro ano",
                        ObjetivosAprendizagem = new List<ObjetivoAprendizagemSimplificadoDto>()
                        {
                            new ObjetivoAprendizagemSimplificadoDto()
                            {
                                Id =343,
                                IdComponenteCurricular =9
                            }
                        }
                    }
                },
            };
        }
    }
}