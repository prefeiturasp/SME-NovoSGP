using Newtonsoft.Json;
using SME.SGP.Dto;
using System;
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
            fixture._clientApi.DefaultRequestHeaders.Clear();
            PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(planoAnualDto), Encoding.UTF8, "application/json");

            var postResult = fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);
            var filtro = new FiltroPlanoAnualDto()
            {
                Ano = 1,
                Bimestre = 1,
                EscolaId = "1",
                TurmaId = 1
            };
            var filtroPlanoAnual = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("api/v1/planos/anual/"),
                Content = filtroPlanoAnual,
            };
            var planoAnualCompletoResponse = fixture._clientApi.SendAsync(request).Result;
            if (planoAnualCompletoResponse.IsSuccessStatusCode)
            {
                var planoAnualCompleto = JsonConvert.DeserializeObject<PlanoCicloCompletoDto>(planoAnualCompletoResponse.Content.ReadAsStringAsync().Result);
                Assert.Equal(planoAnualDto.Descricao, planoAnualCompleto.Descricao);

                var requestValidaPlanoAnualExistente = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("api/v1/planos/anual/validar-existente"),
                    Content = filtroPlanoAnual,
                };
                var planoAnualExistenteResponse = fixture._clientApi.SendAsync(request).Result;
                Assert.True(bool.Parse(planoAnualExistenteResponse.Content.ReadAsStringAsync().Result));
            }
            else
            {
                var erro = postResult.Content.ReadAsStringAsync().Result;
                Assert.True(false, erro);
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
                Bimestre = 1,
                Descricao = "Primeiro bismestre do primeiro ano",
                EscolaId = "095346",
                TurmaId = 2008187,
                ObjetivosAprendizagem = new List<ObjetivoAprendizagemSimplificadoDto>()
                {
                    new ObjetivoAprendizagemSimplificadoDto()
                    {
                        Id=343,
                        IdComponenteCurricular=9
                    }
                }
            };
        }
    }
}