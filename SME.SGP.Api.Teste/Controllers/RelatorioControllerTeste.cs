using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class RelatorioControllerTeste
    {
        [Fact]
        public async Task BuscaAtiva_DeveRetornarOk()
        {
            var filtro = new FiltroRelatorioBuscasAtivasDto();
            var resultadoEsperado = true;

            var mockRelatorioUseCase = new Mock<IRelatorioBuscasAtivasUseCase>();
            mockRelatorioUseCase
                .Setup(x => x.Executar(It.IsAny<FiltroRelatorioBuscasAtivasDto>()))
                .ReturnsAsync(resultadoEsperado);

            var controller = new RelatorioController();

            var result = await controller.BuscaAtiva(filtro, mockRelatorioUseCase.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }
}
