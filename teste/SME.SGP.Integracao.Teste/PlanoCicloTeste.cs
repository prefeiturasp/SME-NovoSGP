using SME.SGP.Infra.Json;
using SME.SGP.Api.Controllers;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class PlanoCicloTeste
    {
        private readonly TestServerFixture _fixture;

        public PlanoCicloTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public void Deve_Incluir_Plano_Ciclo()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PDC_I, Permissao.PDC_C }));

            var planoDeCicloDto = new PlanoCicloDto();
            planoDeCicloDto.Ano = 2019;
            planoDeCicloDto.CicloId = 1;
            planoDeCicloDto.Descricao = "Teste";
            planoDeCicloDto.EscolaId = "1";
            planoDeCicloDto.IdsMatrizesSaber = new List<long> { 1 };
            planoDeCicloDto.IdsObjetivosDesenvolvimento = new List<long> { 1 };

            var jsonParaPost = new StringContent(TransformarEmJson(planoDeCicloDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/planos/ciclo/", jsonParaPost).Result;

            if (postResult.IsSuccessStatusCode)
            {
                var planoCicloCompletoResult = _fixture._clientApi.GetAsync("api/v1/planos/ciclo/2019/1/1").Result;
                if (planoCicloCompletoResult.IsSuccessStatusCode)
                {
                    var planoCicloCompletoDto = SgpJsonSerializer.Deserialize<PlanoCicloCompletoDto>(planoCicloCompletoResult.Content.ReadAsStringAsync().Result);
                    Assert.Equal(planoDeCicloDto.Descricao, planoCicloCompletoDto.Descricao);
                }
                else
                {
                    var erro = postResult.Content.ReadAsStringAsync().Result;
                    Assert.True(false, erro);
                }
            }
            else
            {
                var erro = postResult.Content.ReadAsStringAsync().Result;
                Assert.True(false, erro);
            }
        }

        [Fact, Order(2)]
        public void Deve_Incluir_Plano_Ciclo2()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PDC_I, Permissao.PDC_C }));

            var planoDeCicloDto = new PlanoCicloDto();
            planoDeCicloDto.Ano = 2020;
            planoDeCicloDto.CicloId = 1;
            planoDeCicloDto.Descricao = "Teste";
            planoDeCicloDto.EscolaId = "1";
            planoDeCicloDto.IdsMatrizesSaber = new List<long> { 1 };
            planoDeCicloDto.IdsObjetivosDesenvolvimento = new List<long> { 1 };

            var jsonParaPost = new StringContent(TransformarEmJson(planoDeCicloDto), UnicodeEncoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/planos/ciclo/", jsonParaPost).Result;

            if (postResult.IsSuccessStatusCode)
            {
                var planoCicloCompletoResult = _fixture._clientApi.GetAsync("api/v1/planos/ciclo/2020/1/1").Result;
                if (planoCicloCompletoResult.IsSuccessStatusCode)
                {
                    var planoCicloCompletoDto = SgpJsonSerializer.Deserialize<PlanoCicloCompletoDto>(planoCicloCompletoResult.Content.ReadAsStringAsync().Result);
                    Assert.Equal(planoDeCicloDto.Descricao, planoCicloCompletoDto.Descricao);
                }
                else
                {
                    var erro = postResult.Content.ReadAsStringAsync().Result;
                    Assert.True(false, erro);
                }
            }
            else
            {
                var erro = postResult.Content.ReadAsStringAsync().Result;
                Assert.True(false, erro);
            }
        }

        private string TransformarEmJson(object model)
        {
            return SgpJsonSerializer.Serialize(model);
        }
    }
}