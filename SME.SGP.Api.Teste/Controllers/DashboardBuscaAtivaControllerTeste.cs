using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardBuscaAtivaControllerTeste
    {
        private readonly DashboardBuscaAtivaController _controller;

        private readonly Mock<IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase> _motivosUseCase;
        private readonly Mock<IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase> _procedimentosUseCase;
        private readonly Mock<IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase> _reflexoUseCase;

        public DashboardBuscaAtivaControllerTeste()
        {
            _motivosUseCase = new Mock<IObterQuantidadeBuscaAtivaPorMotivosAusenciaUseCase>();
            _procedimentosUseCase = new Mock<IObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDreUseCase>();
            _reflexoUseCase = new Mock<IObterQuantidadeBuscaAtivaPorReflexoFrequenciaMesUseCase>();

            _controller = new DashboardBuscaAtivaController();
        }

        [Fact(DisplayName = "ObterQuantidadeBuscaAtivaPorMotivosAusencia deve retornar Ok com DTO")]
        public async Task ObterQuantidadeBuscaAtivaPorMotivosAusencia_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoBuscaAtivaDto();

            var dto = new GraficoBuscaAtivaDto();

            _motivosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoBuscaAtivaDto>()))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.ObterQuantidadeBuscaAtivaPorMotivosAusencia(filtro, _motivosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoBuscaAtivaDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDre deve retornar Ok com DTO")]
        public async Task ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDre_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto();
            var lista = new List<GraficoBuscaAtivaDto> { new GraficoBuscaAtivaDto() };

            _procedimentosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto>()))
                .ReturnsAsync(new GraficoBuscaAtivaDto());

            // Act
            var result = await _controller.ObterQuantidadeBuscaAtivaPorProcedimentosTrabalhoDre(filtro, _procedimentosUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoBuscaAtivaDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMes deve retornar Ok com DTO")]
        public async Task ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMes_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoReflexoFrequenciaBuscaAtivaDto();
            var lista = new List<GraficoBuscaAtivaDto> { new GraficoBuscaAtivaDto() };

            _reflexoUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroGraficoReflexoFrequenciaBuscaAtivaDto>()))
                .ReturnsAsync(new GraficoBuscaAtivaDto());

            // Act
            var result = await _controller.ObterQuantidadeBuscaAtivaPorReflexoFrequenciaMes(filtro, _reflexoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoBuscaAtivaDto>(ok.Value);
        }
    }
}
