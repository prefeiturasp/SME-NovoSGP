using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DiasLetivosCalendarioControllerTeste
    {
        private readonly Mock<IObterDiasLetivosPorCalendarioUseCase> _obterDiasLetivosUseCase;
        private readonly DiasLetivosCalendarioController _controller;

        public DiasLetivosCalendarioControllerTeste()
        {
            _obterDiasLetivosUseCase = new Mock<IObterDiasLetivosPorCalendarioUseCase>();
            _controller = new DiasLetivosCalendarioController();
        }

        [Fact(DisplayName = "CalcularDiasLetivos deve retornar Ok com DiasLetivosDto")]
        public async Task CalcularDiasLetivos_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroDiasLetivosDTO();
            var retorno = new DiasLetivosDto();

            _obterDiasLetivosUseCase
                .Setup(s => s.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.CalcularDiasLetivos(filtro, _obterDiasLetivosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DiasLetivosDto>(ok.Value);
        }

        [Fact(DisplayName = "CalcularDiasLetivos deve executar UseCase com filtro correto")]
        public async Task CalcularDiasLetivos_DeveExecutarUseCaseComFiltroCorreto()
        {
            // Arrange
            var filtro = new FiltroDiasLetivosDTO();
            var retorno = new DiasLetivosDto();

            _obterDiasLetivosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroDiasLetivosDTO>()))
                .ReturnsAsync(retorno);

            // Act
            await _controller.CalcularDiasLetivos(filtro, _obterDiasLetivosUseCase.Object);

            // Assert
            _obterDiasLetivosUseCase.Verify(
                s => s.Executar(filtro),
                Times.Once
            );
        }

        [Fact(DisplayName = "CalcularDiasLetivos deve retornar dados calculados")]
        public async Task CalcularDiasLetivos_DeveRetornarDadosCalculados()
        {
            // Arrange
            var filtro = new FiltroDiasLetivosDTO();
            var retorno = new DiasLetivosDto();

            _obterDiasLetivosUseCase
                .Setup(s => s.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.CalcularDiasLetivos(filtro, _obterDiasLetivosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dados = Assert.IsType<DiasLetivosDto>(ok.Value);
            Assert.Same(retorno, dados);
        }
    }
}