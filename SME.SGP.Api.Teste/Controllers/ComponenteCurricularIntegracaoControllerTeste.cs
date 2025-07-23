using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class ComponenteCurricularIntegracaoControllerTeste
    {
        private readonly ComponenteCurricularIntegracaoController _controller;
        private readonly Mock<IObterComponenteCurricularLancaNotaUseCase> _obterComponenteCurricularLancaNotaUseCase = new();

        public ComponenteCurricularIntegracaoControllerTeste()
        {
            _controller = new ComponenteCurricularIntegracaoController();
        }

        [Fact]
        public async Task VerificarComponenteLancaNota_DeveRetornarOkComResultadoEsperado()
        {
            // Arrange
            long componenteCurricularId = 1001;
            var retornoEsperado = true;

            _obterComponenteCurricularLancaNotaUseCase
                .Setup(x => x.Executar(componenteCurricularId))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _controller.VerificarComponenteLancaNota(componenteCurricularId, _obterComponenteCurricularLancaNotaUseCase.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode ?? 200);
            Assert.IsType<bool>(okResult.Value);
            Assert.Equal(retornoEsperado, (bool)okResult.Value);
        }
    }
}
