using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
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
    public class DiasLetivosCalendarioTeste
    {
        private readonly TestServerFixture _fixture;

        public DiasLetivosCalendarioTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public void Deve_Incluir_Calendario_Fundamental_E_Eventos_NaoLetivos_Retornar_Acima_200_dias_letivos()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[]
                {
                    Permissao.TCE_I,
                    Permissao.PE_I,
                    Permissao.C_C
                })
            );
            var ano = 2019;

            var tipoCalendario = new TipoCalendarioDto
            {
                AnoLetivo = ano,
                DescricaoPeriodo = "teste",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = "teste",
                Periodo = Periodo.Anual,
                Situacao = true
            };

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(tipoCalendario), Encoding.UTF8, "application/json");
            var postResult = _fixture._clientApi.PostAsync("api/v1/calendarios/tipos", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var periodoEscolar = new PeriodoEscolarListaDto
                {
                    TipoCalendario = 1,
                    Periodos = new List<PeriodoEscolarDto>
                    {
                        new PeriodoEscolarDto
                        {
                            Bimestre = 1,
                            PeriodoInicio = new DateTime(ano,1,30),
                            PeriodoFim = new DateTime(ano,4,30) //61 dias úteis
                        },
                        new PeriodoEscolarDto
                        {
                            Bimestre = 2,
                            PeriodoInicio = new DateTime(ano,5,1),
                            PeriodoFim = new DateTime(ano,6,30) //42 dias úteis
                        },
                        new PeriodoEscolarDto
                        {
                            Bimestre = 3,
                            PeriodoInicio = new DateTime(ano,8,1),//43 dias úteis
                            PeriodoFim = new DateTime(ano,9,30)
                        },
                        new PeriodoEscolarDto
                        {
                            Bimestre = 4,
                            PeriodoInicio = new DateTime(ano,10,1),//53 dias úteis
                            PeriodoFim = new DateTime(ano,12,15) //aumentar
                        },
                    }
                };

                var jsonParaPost2 = new StringContent(JsonConvert.SerializeObject(periodoEscolar), Encoding.UTF8, "application/json");
                var postResult2 = _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost2).Result;
                Assert.True(postResult2.IsSuccessStatusCode);

                var filtro = new FiltroDiasLetivosDTO()
                {
                    TipoCalendarioId = 1
                };

                var filtroPeriodoEscolar = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
                var diasLetivosResponse = _fixture._clientApi.PostAsync("api/v1/calendarios/dias-letivos", filtroPeriodoEscolar).Result;
                if (diasLetivosResponse.IsSuccessStatusCode)
                {
                    var diasLetivos = JsonConvert.DeserializeObject<DiasLetivosDto>(diasLetivosResponse.Content.ReadAsStringAsync().Result);
                    Assert.False(diasLetivos.EstaAbaixoPermitido);
                }
            }
        }
    }
}