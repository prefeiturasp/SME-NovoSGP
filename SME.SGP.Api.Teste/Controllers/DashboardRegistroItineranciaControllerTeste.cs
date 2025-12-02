using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardRegistroItineranciaControllerTeste
    {
        private readonly Mock<IObterDashboardItineranciaVisitasPAAIsUseCase> _visitasPaaisUseCase;
        private readonly Mock<IObterDashboardItineranciaObjetivosUseCase> _objetivosUseCase;

        private readonly DashboardRegistroItineranciaController _controller;

        public DashboardRegistroItineranciaControllerTeste()
        {
            _visitasPaaisUseCase = new Mock<IObterDashboardItineranciaVisitasPAAIsUseCase>();
            _objetivosUseCase = new Mock<IObterDashboardItineranciaObjetivosUseCase>();

            _controller = new DashboardRegistroItineranciaController();
        }

        [Fact(DisplayName = "ObterVisitasPAAIs deve retornar Ok com DashboardItineranciaVisitaPaais")]
        public async Task ObterVisitasPAAIs_DeveRetornarOk()
        {
            // Arrange
            var anoLetivo = 2024;
            var dreId = 1L;
            var ueId = 10L;
            var mes = 6;
            var retorno = new DashboardItineranciaVisitaPaais();

            _visitasPaaisUseCase
                .Setup(s => s.Executar(It.Is<FiltroDashboardItineranciaDto>(f =>
                    f.AnoLetivo == anoLetivo &&
                    f.DreId == dreId &&
                    f.UeId == ueId &&
                    f.Mes == mes)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterVisitasPAAIs(
                anoLetivo,
                dreId,
                ueId,
                mes,
                _visitasPaaisUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DashboardItineranciaVisitaPaais>(ok.Value);
        }

        [Fact(DisplayName = "ObterVisitasPAAIs deve retornar Ok com dados corretos")]
        public async Task ObterVisitasPAAIs_DeveRetornarDadosCorretos()
        {
            // Arrange
            var anoLetivo = 2024;
            var dreId = 1L;
            var ueId = 10L;
            var mes = 6;
            var retorno = new DashboardItineranciaVisitaPaais
            {
                // Inicialize as propriedades conforme necessário
            };

            _visitasPaaisUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroDashboardItineranciaDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterVisitasPAAIs(
                anoLetivo,
                dreId,
                ueId,
                mes,
                _visitasPaaisUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dados = Assert.IsType<DashboardItineranciaVisitaPaais>(ok.Value);
            Assert.Same(retorno, dados);
            
            _visitasPaaisUseCase.Verify(
                s => s.Executar(It.Is<FiltroDashboardItineranciaDto>(f =>
                    f.AnoLetivo == anoLetivo &&
                    f.DreId == dreId &&
                    f.UeId == ueId &&
                    f.Mes == mes)),
                Times.Once
            );
        }

        [Fact(DisplayName = "ObterObjetivos deve retornar Ok com lista")]
        public async Task ObterObjetivos_DeveRetornarOk()
        {
            // Arrange
            var anoLetivo = 2024;
            var dreId = 1L;
            var ueId = 10L;
            var mes = 6;
            var rf = "1234567";
            var retorno = new List<DashboardItineranciaDto>
            {
                new DashboardItineranciaDto()
            };

            _objetivosUseCase
                .Setup(s => s.Executar(It.Is<FiltroDashboardItineranciaDto>(f =>
                    f.AnoLetivo == anoLetivo &&
                    f.DreId == dreId &&
                    f.UeId == ueId &&
                    f.Mes == mes &&
                    f.RF == rf)))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterObjetivos(
                anoLetivo,
                dreId,
                ueId,
                mes,
                rf,
                _objetivosUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<DashboardItineranciaDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterObjetivos deve retornar Ok com dados corretos")]
        public async Task ObterObjetivos_DeveRetornarDadosCorretos()
        {
            // Arrange
            var anoLetivo = 2024;
            var dreId = 1L;
            var ueId = 10L;
            var mes = 6;
            var rf = "1234567";
            var retorno = new List<DashboardItineranciaDto>
            {
                new DashboardItineranciaDto(),
                new DashboardItineranciaDto()
            };

            _objetivosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroDashboardItineranciaDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterObjetivos(
                anoLetivo,
                dreId,
                ueId,
                mes,
                rf,
                _objetivosUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dados = Assert.IsAssignableFrom<IEnumerable<DashboardItineranciaDto>>(ok.Value);
            Assert.Equal(2, ((List<DashboardItineranciaDto>)dados).Count);
            
            _objetivosUseCase.Verify(
                s => s.Executar(It.Is<FiltroDashboardItineranciaDto>(f =>
                    f.AnoLetivo == anoLetivo &&
                    f.DreId == dreId &&
                    f.UeId == ueId &&
                    f.Mes == mes &&
                    f.RF == rf)),
                Times.Once
            );
        }

        [Fact(DisplayName = "ObterObjetivos deve retornar Ok com lista vazia")]
        public async Task ObterObjetivos_DeveRetornarOkComListaVazia()
        {
            // Arrange
            var anoLetivo = 2024;
            var dreId = 1L;
            var ueId = 10L;
            var mes = 6;
            var rf = "1234567";
            var retorno = new List<DashboardItineranciaDto>();

            _objetivosUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroDashboardItineranciaDto>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ObterObjetivos(
                anoLetivo,
                dreId,
                ueId,
                mes,
                rf,
                _objetivosUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dados = Assert.IsAssignableFrom<IEnumerable<DashboardItineranciaDto>>(ok.Value);
            Assert.Empty(dados);
        }
    }
}