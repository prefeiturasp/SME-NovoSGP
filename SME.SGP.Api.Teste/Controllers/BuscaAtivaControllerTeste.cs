using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.Api.Controllers;

namespace SME.SGP.Api.Teste.Controllers
{
    public class BuscaAtivaControllerTeste
    {
        [Fact]
        public async Task ObterRegistrosAcao_DeveRetornarOkComResultado()
        {
            var filtro = new FiltroRegistrosAcaoDto();
            var resultadoEsperado = new PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>
            {
            };

            var useCaseMock = new Mock<IObterRegistrosAcaoUseCase>();
            useCaseMock
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(resultadoEsperado);

            var controller = new BuscaAtivaController();

            var resultado = await controller.ObterRegistrosAcao(filtro, useCaseMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }
}