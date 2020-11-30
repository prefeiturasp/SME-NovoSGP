using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace SME.SGP.Integracao.Teste.EscolaAqui.Dashboard
{
    [Collection("Testserver collection")]
    public class ObterDadosDeLeituraDeComunicadosTeste
    {
        private readonly TestServerFixture fixture;

        public ObterDadosDeLeituraDeComunicadosTeste(TestServerFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Theory(DisplayName = "Requisição para API do App Escola Aqui para busca de dados de leitura de comunicado (DRE/UE)")]
        [InlineData("109300", "099139", 237)]
        [InlineData("", "", 254)]
        [InlineData("109300", "099139", 256)]
        private async Task ObterDadosDeLeituraDeComunicados_ComEntradasValidas_RetornandoListaVaziaOuPreenchida(string codigoDre, string codigoUe, long comunicadoId)
        {
            // Arrange
            var dto = new ObterDadosDeLeituraDeComunicadosDto
            {
                CodigoDre = codigoDre,
                CodigoUe = codigoUe,
                ComunicadoId = comunicadoId
            };

            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var requestParameters = $"?codigoDre={dto.CodigoDre}&codigoUe={dto.CodigoUe}&comunicadoId={dto.ComunicadoId}";
            var result = await fixture._clientApi.GetAsync($"api/v1/ea/dashboard/comunicados/leitura{requestParameters}");

            // Assert
            Assert.True(fixture.ValidarStatusCodeComSucesso(result));
        }

        [Theory(DisplayName = "Requisição para API do App Escola Aqui para busca de dados de leitura de comunicado (DRE/UE)")]
        [InlineData("109300", "099139", 0)]
        [InlineData("", "", -50)]
        private async Task ObterDadosDeLeituraDeComunicados_ComComunicadoIdInexistente_RetornandoErro400(string codigoDre, string codigoUe, long comunicadoId)
        {
            // Arrange
            var dto = new ObterDadosDeLeituraDeComunicadosDto
            {
                CodigoDre = codigoDre,
                CodigoUe = codigoUe,
                ComunicadoId = comunicadoId
            };

            // Act
            fixture._clientApi.DefaultRequestHeaders.Clear();
            var requestParameters = $"?codigoDre={dto.CodigoDre}&codigoUe={dto.CodigoUe}&comunicadoId={dto.ComunicadoId}";
            var result = await fixture._clientApi.GetAsync($"api/v1/ea/dashboard/comunicados/leitura{requestParameters}");

            // Assert
            Assert.IsType<BadRequestErrorMessageResult>(result);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}
