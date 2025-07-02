using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using Xunit;

namespace SME.SGP.Api.Tests.Controllers
{
    public class AcompanhamentoTurmaControllerTeste
    {
        [Fact]
        public async Task Salvar_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<ISalvarAcompanhamentoTurmaUseCase>();
            var dto = new AcompanhamentoTurmaDto();
            var resultadoEsperado = new AcompanhamentoTurma
            {
                TurmaId = 1,
                Semestre = 2,
                ApanhadoGeral = "Texto de teste"
            };

            useCaseMock.Setup(x => x.Executar(dto)).ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoTurmaController();

            // Act
            var resultado = await controller.Salvar(useCaseMock.Object, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task Obter_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterAcompanhamentoTurmaApanhadoGeralUseCase>();
            var filtro = new FiltroAcompanhamentoTurmaApanhadoGeral();
            var resultadoEsperado = new AcompanhamentoTurmaDto();

            useCaseMock.Setup(x => x.Executar(filtro)).ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoTurmaController();

            // Act
            var resultado = await controller.Obter(filtro, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }

        [Fact]
        public async Task ObterParametroQuantidadeImagens_DeveRetornarOkResult_QuandoUseCaseExecutarComSucesso()
        {
            // Arrange
            var useCaseMock = new Mock<IObterParametroQuantidadeImagensPercursoColetivoTurmaUseCase>();
            int ano = 2025;
            var resultadoEsperado = new ParametroQuantidadeUploadImagemDto();

            useCaseMock.Setup(x => x.Executar(ano)).ReturnsAsync(resultadoEsperado);

            var controller = new AcompanhamentoTurmaController();

            // Act
            var resultado = await controller.ObterParametroQuantidadeImagens(ano, useCaseMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(resultadoEsperado, okResult.Value);
        }
    }
}
