using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class DiasLetivosCalendarioTeste
    {
        private readonly TestServerFixture _fixture;
        private readonly int ano = 2019;

        public DiasLetivosCalendarioTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact, Order(1)]
        //public async Task Deve_Incluir_Calendario_Fundamental_E_Retornar_Acima_200_dias_letivos()
        //{
        //    try
        //    {


        //        MontarCabecalho();
        //        TipoCalendarioDto tipoCalendario = AdicionarTipoCalendario(ano);

        //        var jsonParaPost = new StringContent(JsonConvert.SerializeObject(tipoCalendario), Encoding.UTF8, "application/json");
        //        var postResult = await _fixture._clientApi.PostAsync("api/v1/calendarios/tipos", jsonParaPost);

        //        Assert.True(postResult.IsSuccessStatusCode);

        //        if (postResult.IsSuccessStatusCode)
        //        {

        //           PeriodoEscolarListaDto periodoEscolar = AdicionarPerioEscolar(ano);

        //            var jsonParaPost2 = new StringContent(JsonConvert.SerializeObject(periodoEscolar), Encoding.UTF8, "application/json");
        //            var postResult2 = await _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost2);
        //            Assert.True(postResult2.IsSuccessStatusCode);

        //            var filtro = new FiltroDiasLetivosDTO()
        //            {
        //                TipoCalendarioId = 1
        //            };

        //            var filtroPeriodoEscolar = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");
        //            var diasLetivosResponse = await _fixture._clientApi.PostAsync("api/v1/calendarios/dias-letivos", filtroPeriodoEscolar);
        //            if (diasLetivosResponse.IsSuccessStatusCode)
        //            {
        //                var diasLetivos = JsonConvert.DeserializeObject<DiasLetivosDto>(await diasLetivosResponse.Content.ReadAsStringAsync());
        //                Assert.True(diasLetivos.Dias > 0);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        private static PeriodoEscolarListaDto AdicionarPerioEscolar(int ano)
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
            return periodoEscolar;
        }

        private static TipoCalendarioDto AdicionarTipoCalendario(int ano)
        {
            return new TipoCalendarioDto
            {
                AnoLetivo = ano,
                DescricaoPeriodo = "teste",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = "testeCalculoDiaPeriodoEscolar",
                Periodo = Periodo.Anual,
                Situacao = true
            };
        }

        private void MontarCabecalho()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[]
                {
                    Permissao.TCE_I,
                    Permissao.PE_I,
                    Permissao.C_C,
                    Permissao.TE_I,
                    Permissao.E_I
                })
            );
        }
    }
}