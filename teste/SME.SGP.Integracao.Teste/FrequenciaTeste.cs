using SME.SGP.Infra;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class FrequenciaTeste
    {
        private TestServerFixture _fixture;

        public FrequenciaTeste(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        //public void Deve_Obter_Frequencia()
        //{
        //    _fixture = TesteBase.ObtenhaCabecalhoAuthentication(_fixture, new Permissao[] { Permissao.PDA_C, Permissao.TCE_I, Permissao.PE_I, Permissao.CP_I });

        //    TesteBase.AdicionarTipoCalendario(_fixture);
        //    TesteBase.AdicionarPeriodoEscolar(_fixture);
        //    TesteBase.AdicionarAula(_fixture);

        //    var resposta = TesteBase.ExecuteGetAsync(_fixture, $"/api/v1/calendarios/frequencias?aulaId=1");
        //    Assert.True(resposta.IsSuccessStatusCode);
        //}

        [Fact]
        public async Task Deve_Obter_Datas_Das_Aulas()
        {
            _fixture._clientApi.DefaultRequestHeaders.Clear();

            _fixture._clientApi.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _fixture.GerarToken(new Permissao[] { Permissao.PDA_C }));

            var turmaCodigo = "2120528";
            var componenteCurricularCodigo = "1105";

            var result = await _fixture._clientApi.GetAsync($"api/v1/calendarios/frequencias/aulas/datas/turmas/{turmaCodigo}/componente/{componenteCurricularCodigo}");

            Assert.True(_fixture.ValidarStatusCodeComSucesso(result));
        }

    }
}