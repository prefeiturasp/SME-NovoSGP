using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

// Teste do comportamento do controlador

namespace SME.SGP.Api.Teste.Controllers
{
    public class RelatorioControllerTeste
    {
        [Fact]
        public async Task BuscaAtiva_DeveRetornarOk()
        {
            // Arrange: Preparar o filtro e o resultado esperado
            var filtro = new FiltroRelatorioBuscasAtivasDto(); // Pode ser um objeto vazio para simplificar o teste.
            var resultadoEsperado = true;

            // Mock do UseCase
            var mockRelatorioUseCase = new Mock<IRelatorioBuscasAtivasUseCase>();
            mockRelatorioUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroRelatorioBuscasAtivasDto>()))
                .ReturnsAsync(resultadoEsperado);

            // Instância do Controller
            var controller = new RelatorioController();

            // Act: Chamar o método a ser testado
            var result = await controller.BuscaAtiva(filtro, mockRelatorioUseCase.Object);

            // Assert: Verificar o resultado
            var okResult = Assert.IsType<OkObjectResult>(result); // Verifica se o retorno é 200 OK.
            Assert.NotNull(okResult.Value);                      // Verifica se há um valor no retorno.
            Assert.Equal(resultadoEsperado, okResult.Value);     // Compara o valor retornado com o esperado.
        }
    }
}
