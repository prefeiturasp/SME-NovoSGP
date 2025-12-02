using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardDevolutivasControllerTeste
    {
        private readonly DashboardDevolutivasController _controller;

        private readonly Mock<IObterDevolutivasEstimadasEConfirmadasUseCase> _estimadasUseCase;
        private readonly Mock<IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase> _diariosUseCase;
        private readonly Mock<IObterGraficoTotalDevolutivasPorDreUseCase> _dreUseCase;
        private readonly Mock<IObterUltimaConsolidacaoDevolutivaUseCase> _ultimaConsolidacaoUseCase;
        private readonly Mock<IObterQuantidadeTotalDeDevolutivasPorDREUseCase> _totalAnoUseCase;

        public DashboardDevolutivasControllerTeste()
        {
            _estimadasUseCase = new Mock<IObterDevolutivasEstimadasEConfirmadasUseCase>();
            _diariosUseCase = new Mock<IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase>();
            _dreUseCase = new Mock<IObterGraficoTotalDevolutivasPorDreUseCase>();
            _ultimaConsolidacaoUseCase = new Mock<IObterUltimaConsolidacaoDevolutivaUseCase>();
            _totalAnoUseCase = new Mock<IObterQuantidadeTotalDeDevolutivasPorDREUseCase>();

            _controller = new DashboardDevolutivasController();
        }

        [Fact(DisplayName = "ObterDevolutivasEstimadasEConfirmadas deve retornar Ok com lista")]
        public async Task ObterDevolutivasEstimadasEConfirmadas_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoDevolutivasEstimadasEConfirmadasDto();

            var lista = new List<GraficoDevolutivasEstimadasEConfirmadasDto>
            {
                new GraficoDevolutivasEstimadasEConfirmadasDto()
            };

            _estimadasUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(lista);

            // Act
            var result = await _controller.ObterDevolutivasEstimadasEConfirmadas(filtro, _estimadasUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendente deve retornar Ok com lista")]
        public async Task ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendente_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto();

            var lista = new List<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>
            {
                new GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto()
            };

            _diariosUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(lista);

            // Act
            var result = await _controller.ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendente(
                filtro,
                _diariosUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterGraficoDevolutivasPorDre deve retornar Ok com lista")]
        public async Task ObterGraficoDevolutivasPorDre_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroTotalDevolutivasPorDreDto();

            var lista = new List<GraficoBaseDto>
            {
                new GraficoBaseDto()
            };

            _dreUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(lista);

            // Act
            var result = await _controller.ObterGraficoDevolutivasPorDre(filtro, _dreUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterUltimaConsolidacao deve retornar Ok com string")]
        public async Task ObterUltimaConsolidacao_DeveRetornarOk()
        {
            // Arrange
            var anoLetivo = 2024;
            DateTime? valor = new DateTime(2024, 02, 15);

            _ultimaConsolidacaoUseCase
                .Setup(u => u.Executar(anoLetivo))
                .ReturnsAsync(valor);

            // Act
            var result = await _controller.ObterUltimaConsolidacao(anoLetivo, _ultimaConsolidacaoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeTotalDeDevolutivasPorAno deve retornar Ok com lista")]
        public async Task ObterQuantidadeTotalDeDevolutivasPorAno_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDevolutivasDto();

            var lista = new List<GraficoTotalDevolutivasPorAnoDTO>
            {
                new GraficoTotalDevolutivasPorAnoDTO()
            };

            _totalAnoUseCase.Setup(s => s.Executar(filtro)).ReturnsAsync(lista);

            // Act
            var result = await _controller.ObterQuantidadeTotalDeDevolutivasPorAno(filtro, _totalAnoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoTotalDevolutivasPorAnoDTO>>(ok.Value);
        }
    }
}
