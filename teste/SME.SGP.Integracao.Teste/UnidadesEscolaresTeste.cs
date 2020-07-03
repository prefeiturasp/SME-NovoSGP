using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class UnidadesEscolaresTeste
    {
        private readonly TestServerFixture fixture;
        private string codigoDRE = "108200";
        private string codigoUE = "019303";
        private FiltroFuncionarioDto filtro = new FiltroFuncionarioDto();

        public UnidadesEscolaresTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar Funcionarios")]
        [Trait("Unidades Escolares", "Deve retornar os funcionarios por filtro")]
        public async Task Deve_Retornar_Funcionarios_Por_Filtro()
        {
            // Arrange
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { Permissao.AS_C }));
            filtro.AtualizaCodigoDre(codigoDRE);
            filtro.AtualizaCodigoUe(codigoUE);
            var jsonFiltro = new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json");

            // Act
            var result = await fixture._clientApi.PostAsync($"api/v1/unidades-escolares/funcionarios", jsonFiltro);

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}