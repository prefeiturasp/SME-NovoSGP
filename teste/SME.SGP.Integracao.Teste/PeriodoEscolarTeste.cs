using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
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
    public class PeriodoEscolarTeste
    {
        private readonly TestServerFixture _fixture;

        public PeriodoEscolarTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        [Fact, Order(1)]
        public void Deve_Incluir_Tipo_Calendario_e_Periodo_Escolar()
        {
            try
            {
                _fixture._clientApi.DefaultRequestHeaders.Clear();

                _fixture._clientApi.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.C_C, Permissao.C_I, Permissao.C_E }));

                var tipoCalendarioDto = new TipoCalendarioDto();
                tipoCalendarioDto.AnoLetivo = 2019;
                tipoCalendarioDto.Nome = "Teste 1";
                tipoCalendarioDto.Periodo = Periodo.Anual;
                tipoCalendarioDto.Modalidade = ModalidadeTipoCalendario.FundamentalMedio;
                tipoCalendarioDto.Situacao = true;

                var tipoCalendarioDto2 = new TipoCalendarioDto();
                tipoCalendarioDto2.AnoLetivo = 2019;
                tipoCalendarioDto2.Nome = "Teste 2";
                tipoCalendarioDto2.Periodo = Periodo.Semestral;
                tipoCalendarioDto2.Modalidade = ModalidadeTipoCalendario.FundamentalMedio;
                tipoCalendarioDto2.Situacao = true;

                var jsonParaPost = new StringContent(JsonConvert.SerializeObject(tipoCalendarioDto), UnicodeEncoding.UTF8, "application/json");
                var postResult = _fixture._clientApi.PostAsync("api/v1/tipo-calendario/", jsonParaPost).Result;


                var jsonParaPost2 = new StringContent(JsonConvert.SerializeObject(tipoCalendarioDto2), UnicodeEncoding.UTF8, "application/json");
                var postResult2 = _fixture._clientApi.PostAsync("api/v1/tipo-calendario/", jsonParaPost2).Result;

                Assert.True(postResult.IsSuccessStatusCode);
                Assert.True(postResult2.IsSuccessStatusCode);

                _fixture._clientApi.DefaultRequestHeaders.Clear();

                _fixture._clientApi.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_A }));

                PeriodoEscolarListaDto Dto = ObtenhaDto();

                var jsonParaPost3 = new StringContent(JsonConvert.SerializeObject(Dto), Encoding.UTF8, "application/json");

                var postResult3 = _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost3).Result;

                Assert.True(postResult3.IsSuccessStatusCode);

            }
            catch (AggregateException ae)
            {
                throw new Exception("Erros: " + string.Join(",", ae.InnerExceptions));
            }
        }

        [Fact, Order(2)]
        public void Deve_Consultar_Periodo_Escolar()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_C }));
                       
            var getResult = _fixture._clientApi.GetAsync("api/v1/periodo-escolar?codigoTipoCalendario=1").Result;

            Assert.True(getResult.IsSuccessStatusCode);
        }

        private PeriodoEscolarListaDto ObtenhaDto()
        {
            return new PeriodoEscolarListaDto
            {
                AnoBase = DateTime.Now.Year,
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTime.Now,
                        PeriodoFim = DateTime.Now.AddMinutes(1)
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTime.Now.AddMinutes(2),
                        PeriodoFim = DateTime.Now.AddMinutes(3)
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTime.Now.AddMinutes(4),
                        PeriodoFim = DateTime.Now.AddMinutes(5)
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTime.Now.AddMinutes(6),
                        PeriodoFim = DateTime.Now.AddMinutes(7)
                    },
                }
            };
        }

    }
}
