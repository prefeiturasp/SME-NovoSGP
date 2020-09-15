using SME.SGP.Infra.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

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

        //[Fact]
        //public async Task Deve_Incluir_Calendario_E_Feriados_Moveis()
        //{
        //    _fixture._clientApi.DefaultRequestHeaders.Clear();

        //    _fixture._clientApi.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.TCE_C, Permissao.TCE_I, Permissao.TCE_E, Permissao.TF_C }));

        //    var calendarioParaIncluir = new TipoCalendarioDto()
        //    {
        //        AnoLetivo = 2019,
        //        DescricaoPeriodo = "Teste",
        //        Modalidade = Dominio.ModalidadeTipoCalendario.EJA,
        //        Nome = "Calendário de teste",
        //        Periodo = Dominio.Periodo.Anual,
        //        Situacao = true
        //    };

        //    var jsonParaPostCalendario = new StringContent(TransformarEmJson(calendarioParaIncluir), Encoding.UTF8, "application/json");
        //    var postResultIncluiCalendario = await _fixture._clientApi.PostAsync("api/v1/calendarios/tipos/", jsonParaPostCalendario);

        //    Assert.True(postResultIncluiCalendario.IsSuccessStatusCode);

        //    if (postResultIncluiCalendario.IsSuccessStatusCode)
        //    {
        //        var buscarTodosCalendariosResultado = await _fixture._clientApi.GetAsync($"api/v1/calendarios/tipos");

        //        Assert.True(buscarTodosCalendariosResultado.IsSuccessStatusCode);
        //        if (buscarTodosCalendariosResultado.IsSuccessStatusCode)
        //        {
        //            var dtoTodos = SgpJsonSerializer.Deserialize<IEnumerable<TipoCalendarioDto>>(await buscarTodosCalendariosResultado.Content.ReadAsStringAsync());
        //            Assert.True(dtoTodos.Any());

        //            var filtroFeriadoCalendarioDto = new FiltroFeriadoCalendarioDto() { Tipo = Dominio.TipoFeriadoCalendario.Movel, Ano = 2019 };

        //            var jsonParaPostFiltroFeriados = new StringContent(TransformarEmJson(filtroFeriadoCalendarioDto), Encoding.UTF8, "application/json");

        //            Thread.Sleep(2000);

        //            var postResultBuscaFeriados = await _fixture._clientApi.PostAsync("api/v1/calendarios/feriados/listar", jsonParaPostFiltroFeriados);

        //            Assert.True(postResultBuscaFeriados.IsSuccessStatusCode);
        //            if (postResultBuscaFeriados.IsSuccessStatusCode)
        //            {
        //                var dtoFeriados = SgpJsonSerializer.Deserialize<IEnumerable<FeriadoCalendarioDto>>(await postResultBuscaFeriados.Content.ReadAsStringAsync());
        //                Assert.True(dtoFeriados.Count() == 4);
        //            }
        //        }
        //    }
        //}

        [Fact, Order(1)]
        public async Task Deve_Consultar_Tipos_Calendario()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.TCE_C}));

            var getResult = await _fixture._clientApi.GetAsync("api/v1/calendarios/tipos/anos-letivos?descricao=2020");

            Assert.True(getResult.IsSuccessStatusCode);
        }

            private string TransformarEmJson(object model)
        {
            return SgpJsonSerializer.Serialize(model);
        }
    }
}