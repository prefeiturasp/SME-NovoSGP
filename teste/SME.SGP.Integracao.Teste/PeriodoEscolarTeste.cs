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
        public void Deve_Incluir_Periodo_Escolar()
        {
            try
            {
                _fixture._clientApi.DefaultRequestHeaders.Clear();

                _fixture._clientApi.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PA_I, Permissao.PA_C }));

                PeriodoEscolarListaDto Dto = ObtenhaDto();

                var jsonParaPost = new StringContent(JsonConvert.SerializeObject(Dto), Encoding.UTF8, "application/json");

                var postResult = _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost).Result;

                Assert.True(postResult.IsSuccessStatusCode);

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
