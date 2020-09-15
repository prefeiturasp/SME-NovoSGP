using SME.SGP.Infra.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class TipoCalendarioEscolarTeste
    {
        private readonly TestServerFixture _fixture;

        public TipoCalendarioEscolarTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public async Task Deve_Incluir_Excluir_Consular_Tipo_Calendario()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.TCE_C, Permissao.TCE_I, Permissao.TCE_E }));

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

            var jsonParaPost = new StringContent(TransformarEmJson(tipoCalendarioDto), UnicodeEncoding.UTF8, "application/json");
            var postResult = await _fixture._clientApi.PostAsync("api/v1/calendarios/tipos/", jsonParaPost);

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var jsonParaPost2 = new StringContent(TransformarEmJson(tipoCalendarioDto2), UnicodeEncoding.UTF8, "application/json");
                var postResult2 = await _fixture._clientApi.PostAsync("api/v1/calendarios/tipos/", jsonParaPost2);
                Assert.True(postResult2.IsSuccessStatusCode);

                if (postResult2.IsSuccessStatusCode)
                {
                    var getAllResult = await _fixture._clientApi.GetAsync($"api/v1/calendarios/tipos/");
                    var dtoTodos = SgpJsonSerializer.Deserialize<IEnumerable<TipoCalendarioDto>>(getAllResult.Content.ReadAsStringAsync().Result);

                    Assert.True(dtoTodos.Any());

                    var feriadoParaExcluir = dtoTodos.ElementAt(0);

                    var ids = new long[1];
                    ids[0] = feriadoParaExcluir.Id;

                    var jsonDelete = new StringContent(SgpJsonSerializer.Serialize(ids), UnicodeEncoding.UTF8, "application/json");
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = jsonDelete,
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri($"{ _fixture._clientApi.BaseAddress}api/v1/calendarios/tipos/")
                    };

                    var deleteResult = await _fixture._clientApi.SendAsync(request);

                    Assert.False(deleteResult.IsSuccessStatusCode);
                    var feriadoParaConsultar = dtoTodos.ElementAt(1);
                    var getOneResult = await _fixture._clientApi.GetAsync($"api/v1/calendarios/tipos/{feriadoParaConsultar.Id}");

                    Assert.True(getOneResult.IsSuccessStatusCode);

                    var dtoUm = SgpJsonSerializer.Deserialize<TipoCalendarioCompletoDto>(getOneResult.Content.ReadAsStringAsync().Result);

                    Assert.True(dtoUm.Id == feriadoParaConsultar.Id);
                }
            }
        }

        private string TransformarEmJson(object model)
        {
            return SgpJsonSerializer.Serialize(model);
        }
    }
}