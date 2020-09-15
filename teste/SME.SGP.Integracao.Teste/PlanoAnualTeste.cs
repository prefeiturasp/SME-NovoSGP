using SME.SGP.Infra.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class PlanoAnualTeste
    {
        private readonly TestServerFixture _fixture;

        public PlanoAnualTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        //[Fact(DisplayName = "Incluir plano anual ", Skip = "Quebrando os testes na versão v2.0"), Order(3)]
        //public void DeveIncluirPlanoAnual()
        //{
        //    try
        //    {
        //        _fixture._clientApi.DefaultRequestHeaders.Clear();

        //        _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_C }));

        //        PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();

        //        var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(planoAnualDto), Encoding.UTF8, "application/json");

        //        var postResult = _fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

        //        Assert.True(postResult.IsSuccessStatusCode);
        //        var filtro = new FiltroPlanoAnualDto()
        //        {
        //            AnoLetivo = 2019,
        //            Bimestre = 1,
        //            EscolaId = "095346",
        //            TurmaId = "2008187",
        //            ComponenteCurricularEolId = 9
        //        };
        //        var filtroPlanoAnual = new StringContent(SgpJsonSerializer.Serialize(filtro), Encoding.UTF8, "application/json");

        //        var planoAnualCompletoResponse = _fixture._clientApi.PostAsync("api/v1/planos/anual/obter", filtroPlanoAnual).Result;
        //        if (planoAnualCompletoResponse.IsSuccessStatusCode)
        //        {
        //            var planoAnualCompleto = SgpJsonSerializer.Deserialize<PlanoCicloCompletoDto>(planoAnualCompletoResponse.Content.ReadAsStringAsync().Result);
        //            Assert.Contains(planoAnualDto.Bimestres, c => c.Descricao == planoAnualCompleto.Descricao);

        //            var planoAnualExistenteResponse = _fixture._clientApi.PostAsync("api/v1/planos/anual/validar-existente", filtroPlanoAnual).Result;
        //            Assert.True(bool.Parse(planoAnualExistenteResponse.Content.ReadAsStringAsync().Result));
        //        }
        //        else
        //        {
        //            var erro = postResult.Content.ReadAsStringAsync().Result;
        //            Assert.True(false, erro);
        //        }
        //    }
        //    catch (AggregateException ae)
        //    {
        //        throw new Exception("Erros: " + string.Join(",", ae.InnerExceptions));
        //    }
        //}

        [Fact, Order(4)]
        public void NaoDeveIncluirPlanoAnualEExibirMensagemErro()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_C }));

            PlanoAnualDto planoAnualDto = CriarDtoPlanoAnual();
            planoAnualDto.EscolaId = null;
            var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(planoAnualDto), Encoding.UTF8, "application/json");

            var postResult = _fixture._clientApi.PostAsync("api/v1/planos/anual/", jsonParaPost).Result;

            Assert.False(postResult.IsSuccessStatusCode);
            var jsonErro = postResult.Content.ReadAsStringAsync().Result;
            var retornoBase = SgpJsonSerializer.Deserialize<RetornoBaseDto>(jsonErro);
            Assert.Contains(retornoBase.Mensagens, c => c.Equals("A escola deve ser informada"));
        }

        private static PlanoAnualDto CriarDtoPlanoAnual()
        {
            return new PlanoAnualDto()
            {
                AnoLetivo = 2019,
                EscolaId = "095346",
                TurmaId = 2008187,
                ComponenteCurricularEolId = 9,
                Bimestres = new List<BimestrePlanoAnualDto>
                {
                    new BimestrePlanoAnualDto
                    {
                        Bimestre = 1,
                        Descricao = "Primeiro bimestre do primeiro ano",
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