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
    public class FeriadoCalendarioEscolarTeste
    {
        private readonly TestServerFixture _fixture;

        public FeriadoCalendarioEscolarTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact, Order(1)]
        public async Task Deve_Incluir_Excluir_Consular_Feriado_Calendario()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.TF_A, Permissao.TF_I, Permissao.TF_E, Permissao.TF_C }));

            var feriadoCalendarioDto = new FeriadoCalendarioDto
            {
                DataFeriado = DateTime.Now,
                Nome = "Feriado do dia de hoje teste 1",
                Ativo = true,
                Abrangencia = AbrangenciaFeriadoCalendario.Nacional,
                Tipo = TipoFeriadoCalendario.Fixo
            };

            var feriadoCalendarioDto2 = new FeriadoCalendarioDto
            {
                DataFeriado = DateTime.Now,
                Nome = "Feriado do dia de hoje teste 2",
                Ativo = true,
                Abrangencia = AbrangenciaFeriadoCalendario.Estadual,
                Tipo = TipoFeriadoCalendario.Fixo
            };

            var filtro = new FiltroFeriadoCalendarioDto
            {
                Nome = "hoje"
            };

            var jsonParaPost = new StringContent(TransformarEmJson(feriadoCalendarioDto), Encoding.UTF8, "application/json");
            var postResult = await _fixture._clientApi.PostAsync("api/v1/calendarios/feriados/", jsonParaPost);

            Assert.True(postResult.IsSuccessStatusCode);

            if (postResult.IsSuccessStatusCode)
            {
                var jsonParaPost2 = new StringContent(TransformarEmJson(feriadoCalendarioDto2), Encoding.UTF8, "application/json");
                var postResult2 = await _fixture._clientApi.PostAsync("api/v1/calendarios/feriados/", jsonParaPost2);
                Assert.True(postResult2.IsSuccessStatusCode);

                if (postResult2.IsSuccessStatusCode)
                {
                    var ids = new int[1];
                    ids[0] = 12;
                    var jsonDelete = new StringContent(SgpJsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = jsonDelete,
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri($"{ _fixture._clientApi.BaseAddress}api/v1/calendarios/feriados/")
                    };

                    var deleteResult = await _fixture._clientApi.SendAsync(request);

                    Assert.True(deleteResult.IsSuccessStatusCode);

                    var jsonGetAll = new StringContent(SgpJsonSerializer.Serialize(filtro), Encoding.UTF8, "application/json");
                    var getAllResult = await _fixture._clientApi.PostAsync($"api/v1/calendarios/feriados/listar", jsonGetAll);
                    var dtoTodos = SgpJsonSerializer.Deserialize<IEnumerable<FeriadoCalendarioDto>>(getAllResult.Content.ReadAsStringAsync().Result);

                    Assert.True(dtoTodos.Any());

                    var getOneResult = await _fixture._clientApi.GetAsync($"api/v1/calendarios/feriados/{dtoTodos.FirstOrDefault().Id}");
                    var dtoUm = SgpJsonSerializer.Deserialize<TipoCalendarioCompletoDto>(getOneResult.Content.ReadAsStringAsync().Result);

                    Assert.NotNull(dtoUm.Nome);
                }
            }
        }

        private string TransformarEmJson(object model)
        {
            return SgpJsonSerializer.Serialize(model);
        }
    }
}