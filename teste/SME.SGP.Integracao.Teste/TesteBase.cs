using SME.SGP.Infra.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    public static class TesteBase
    {
        //TODO VOLTAR A UTILIZAR QUANDO O TESTE DE USUÁRIO FUNCIONAR
        //public static void AdicionarAula(TestServerFixture _fixture)
        //{
        //    var aula = new AulaDto
        //    {
        //        UeId = "094765",
        //        DisciplinaId = "7",
        //        TurmaId = "1992725",
        //        TipoCalendarioId = 1,
        //        TipoAula = Dominio.TipoAula.Normal,
        //        Quantidade = 2,
        //        DataAula = DateTime.Now.Date,
        //        RecorrenciaAula = RecorrenciaAula.AulaUnica
        //    };

        //    var respostaCadastroAula = ExecutePostAsync(_fixture, $"api/v1/calendarios/professores/aulas", aula);
        //    Assert.True(respostaCadastroAula.IsSuccessStatusCode);
        //}

        public static PeriodoEscolarListaDto AdicionarPeriodoEscolar(TestServerFixture _fixture)
        {
            _fixture = ObtenhaCabecalhoAuthentication(_fixture, new Permissao[] { Permissao.PE_I });

            PeriodoEscolarListaDto dto = ObterPeriodoEscolarDto();

            var postResult = ExecutePostAsync(_fixture, "api/v1/periodo-escolar", dto);
            Assert.True(postResult.IsSuccessStatusCode);
            return dto;
        }

        public static void AdicionarTipoCalendario(TestServerFixture _fixture)
        {
            _fixture = ObtenhaCabecalhoAuthentication(_fixture, new Permissao[] { Permissao.TCE_I });

            var tipoCalendarioDto = new TipoCalendarioDto();
            tipoCalendarioDto.AnoLetivo = 2019;
            tipoCalendarioDto.Nome = "Teste Periodo Escolar";
            tipoCalendarioDto.Periodo = Periodo.Anual;
            tipoCalendarioDto.Modalidade = ModalidadeTipoCalendario.FundamentalMedio;
            tipoCalendarioDto.Situacao = true;

            var postResult = ExecutePostAsync(_fixture, "api/v1/calendarios/tipos", tipoCalendarioDto);
            Assert.True(postResult.IsSuccessStatusCode);
        }

        public static HttpResponseMessage ExecuteDeleteAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.DeleteAsync(Url).Result;
        }

        public static HttpResponseMessage ExecuteDeleteAsync(TestServerFixture _fixture, string Url, object ObjetoEnviar)
        {
            var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(ObjetoEnviar), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_fixture._clientApi.BaseAddress.AbsoluteUri + Url),
                Content = jsonParaPost
            };

            return _fixture._clientApi.SendAsync(request).Result;
        }

        public static HttpResponseMessage ExecuteGetAsync(TestServerFixture _fixture, string Url)
        {
            return _fixture._clientApi.GetAsync(Url).Result;
        }

        public static HttpResponseMessage ExecutePostAsync(TestServerFixture _fixture, string Url, object ObjetoEnviar)
        {
            var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(ObjetoEnviar), Encoding.UTF8, "application/json");

            return _fixture._clientApi.PostAsync(Url, jsonParaPost).Result;
        }

        public static HttpResponseMessage ExecutePutAsync(TestServerFixture _fixture, string Url, object ObjetoEnviar)
        {
            var jsonParaPost = new StringContent(SgpJsonSerializer.Serialize(ObjetoEnviar), Encoding.UTF8, "application/json");

            return _fixture._clientApi.PutAsync(Url, jsonParaPost).Result;
        }


        public static TestServerFixture ObtenhaCabecalhoAuthentication(TestServerFixture _fixture, Permissao[] permissoes, string usuario = "teste", string codigoRf = "123", string perfil = "")
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(permissoes, usuario, codigoRf, perfil));

            return _fixture;
        }

        private static PeriodoEscolarListaDto ObterPeriodoEscolarDto()
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