using SME.SGP.Infra;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Integracao.Teste
{
    [Collection("Testserver collection")]
    public class UnidadesEscolaresTeste
    {
        private readonly TestServerFixture fixture;
        private string codigoDRE = "000892";
        private string codigoUE = "";

        public UnidadesEscolaresTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(DisplayName = "Retornar Funcionarios")]
        [Trait("Unidades Escolares", "Deve retornar os funcionarios filtrados por DREs e UEs")]
        public async Task Deve_Retornar_Funcionarios_Por_Parametros_De_Filtro()
        {
            // Arrange
            fixture._clientApi.DefaultRequestHeaders.Clear();
            fixture._clientApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.GerarToken(new Permissao[] { }));

            // Act
            var result = await fixture._clientApi.GetAsync($"api/v1/unidades-escolares/dresId/{codigoDRE}/ueId/{codigoUE}/functionarios");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }
    }
}