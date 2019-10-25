using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class TipoCalendarioTeste
    {
        private readonly TestServerFixture _fixture;

        public TipoCalendarioTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Deve_Incluir_Calendario_E_Feriados_Moveis()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.C_C, Permissao.C_I, Permissao.C_E }));

            var calendarioParaIncluir = new TipoCalendarioDto()
            {
                AnoLetivo = 2019,
                DescricaoPeriodo = "Teste",
                Modalidade = Dominio.ModalidadeTipoCalendario.EJA,
                Nome = "Calendário de teste",
                Periodo = Dominio.Periodo.Anual,
                Situacao = true
            };

            var jsonParaPostCalendario = new StringContent(TransformarEmJson(calendarioParaIncluir), Encoding.UTF8, "application/json");
            var postResultIncluiCalendario = await _fixture._clientApi.PostAsync("api/v1/tipo-calendario/", jsonParaPostCalendario);

            Assert.True(postResultIncluiCalendario.IsSuccessStatusCode);

            if (postResultIncluiCalendario.IsSuccessStatusCode)
            {
                var buscarTodosCalendariosResultado = await _fixture._clientApi.GetAsync($"api/v1/tipo-calendario/");

                Assert.True(buscarTodosCalendariosResultado.IsSuccessStatusCode);
                if (buscarTodosCalendariosResultado.IsSuccessStatusCode)
                {
                    var dtoTodos = JsonConvert.DeserializeObject<IEnumerable<TipoCalendarioDto>>(await buscarTodosCalendariosResultado.Content.ReadAsStringAsync());
                    Assert.True(dtoTodos.Count() == 1);

                    var filtroFeriadoCalendarioDto = new FiltroFeriadoCalendarioDto() { Tipo = Dominio.TipoFeriadoCalendario.Movel, Ano = 2019 };

                    var jsonParaPostFiltroFeriados = new StringContent(TransformarEmJson(filtroFeriadoCalendarioDto), Encoding.UTF8, "application/json");
                    var postResultBuscaFeriados = await _fixture._clientApi.PostAsync("api/v1/calendarios/feriados/listar", jsonParaPostFiltroFeriados);

                    Assert.True(postResultBuscaFeriados.IsSuccessStatusCode);
                    if (postResultBuscaFeriados.IsSuccessStatusCode)
                    {
                        var dtoFeriados = JsonConvert.DeserializeObject<IEnumerable<FeriadoCalendarioDto>>(await postResultBuscaFeriados.Content.ReadAsStringAsync());
                        Assert.True(dtoFeriados.Count() == 4);
                    }
                }
            }
        }

        private string TransformarEmJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}