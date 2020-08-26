using Newtonsoft.Json;
using SME.SGP.Dominio;
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
    public class PeriodoEscolarTeste
    {
        private readonly TestServerFixture _fixture;

        public PeriodoEscolarTeste(TestServerFixture fixture)
        {
            this._fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
        }

        //[Fact, Order(2)]
        //public void Deve_Consultar_Periodo_Escolar()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PE_I, Permissao.PE_C }));

        //    var getResult = _fixture._clientApi.GetAsync("api/v1/periodo-escolar?codigoTipoCalendario=1").Result;

        //    Assert.True(getResult.IsSuccessStatusCode);
        //}

        //[Fact(DisplayName ="Incluir tipo de calendário e período escola e editar o periodo escolar", Skip ="Quebrando os testes na versão v2.0"), Order(2)]
        //public void Deve_Incluir_Tipo_Calendario_e_Periodo_Escolar_e_Editar_Periodo_Escolar()
        //{
        //    try
        //    {
        //        _fixture._clientApi.DefaultRequestHeaders.Clear();

        //        _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.TCE_I }));

        //        AdicionarTipoCalendario();

        //        _fixture._clientApi.DefaultRequestHeaders.Clear();

        //        _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PE_I, Permissao.PE_A }));

        //        PeriodoEscolarListaDto Dto = AdicionarPeriodo();

        //        EditarPeriodo(Dto);
        //    }
        //    catch (AggregateException ae)
        //    {
        //        throw new Exception("Erros: " + string.Join(",", ae.InnerExceptions));
        //    }
        //}

        private PeriodoEscolarListaDto AdicionarPeriodo()
        {
            PeriodoEscolarListaDto Dto = ObtenhaDto();

            var jsonParaPost2 = new StringContent(JsonConvert.SerializeObject(Dto), Encoding.UTF8, "application/json");

            var postResult2 = _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost2).Result;

            Assert.True(postResult2.IsSuccessStatusCode);
            return Dto;
        }

        private void AdicionarTipoCalendario()
        {
            var tipoCalendarioDto = new TipoCalendarioDto();
            tipoCalendarioDto.AnoLetivo = 2019;
            tipoCalendarioDto.Nome = "Teste Periodo Escolar";
            tipoCalendarioDto.Periodo = Periodo.Anual;
            tipoCalendarioDto.Modalidade = ModalidadeTipoCalendario.FundamentalMedio;
            tipoCalendarioDto.Situacao = true;

            var jsonParaPost = new StringContent(JsonConvert.SerializeObject(tipoCalendarioDto), UnicodeEncoding.UTF8, "application/json");
            var postResult = _fixture._clientApi.PostAsync("api/v1/calendarios/tipos", jsonParaPost).Result;

            Assert.True(postResult.IsSuccessStatusCode);
        }

        private void EditarPeriodo(PeriodoEscolarListaDto Dto)
        {
            Dto.Periodos[0].Id = 1;
            Dto.Periodos[0].PeriodoInicio = DateTime.Now.AddMinutes(0);
            Dto.Periodos[0].PeriodoFim = DateTime.Now.AddMinutes(1);
            Dto.Periodos[1].Id = 2;
            Dto.Periodos[1].PeriodoInicio = DateTime.Now.AddMinutes(2);
            Dto.Periodos[1].PeriodoFim = DateTime.Now.AddMinutes(3);
            Dto.Periodos[2].Id = 3;
            Dto.Periodos[2].PeriodoInicio = DateTime.Now.AddMinutes(4);
            Dto.Periodos[2].PeriodoFim = DateTime.Now.AddMinutes(5);
            Dto.Periodos[3].Id = 4;
            Dto.Periodos[3].PeriodoInicio = DateTime.Now.AddMinutes(6);
            Dto.Periodos[3].PeriodoFim = DateTime.Now.AddMinutes(7);

            var jsonParaPost3 = new StringContent(JsonConvert.SerializeObject(Dto), Encoding.UTF8, "application/json");

            var postResult3 = _fixture._clientApi.PostAsync("api/v1/periodo-escolar", jsonParaPost3).Result;

            Assert.True(postResult3.IsSuccessStatusCode);
        }

        private PeriodoEscolarListaDto ObtenhaDto()
        {
            return new PeriodoEscolarListaDto
            {
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