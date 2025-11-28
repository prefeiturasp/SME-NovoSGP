using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardFrequenciaControllerTeste
    {
        private readonly Mock<IObterDataConsolidacaoFrequenciaUseCase> _ultimaConsolidacaoUseCase;
        private readonly Mock<IObterModalidadesAnoUseCase> _modalidadesAnoUseCase;
        private readonly Mock<IObterDashboardFrequenciaPorAnoUseCase> _dashAnoUseCase;
        private readonly Mock<IObterDadosDashboardFrequenciaPorDreUseCase> _freqPorDreUseCase;
        private readonly Mock<IObterDashboardFrequenciaAusenciasPorMotivoUseCase> _ausenciasMotivoUseCase;
        private readonly Mock<IObterDadosDashboardAusenciasComJustificativaUseCase> _ausJustUseCase;
        private readonly Mock<IObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase> _freqDiariaUseCase;
        private readonly Mock<IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase> _freqSemanalMensalUseCase;
        private readonly Mock<IObterFiltroSemanaUseCase> _filtroSemanaUseCase;
        private readonly Mock<IExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase> _consolidacaoUseCase;

        private readonly DashboardFrequenciaController _controller;

        public DashboardFrequenciaControllerTeste()
        {
            _ultimaConsolidacaoUseCase = new();
            _modalidadesAnoUseCase = new();
            _dashAnoUseCase = new();
            _freqPorDreUseCase = new();
            _ausenciasMotivoUseCase = new();
            _ausJustUseCase = new();
            _freqDiariaUseCase = new();
            _freqSemanalMensalUseCase = new();
            _filtroSemanaUseCase = new();
            _consolidacaoUseCase = new();

            _controller = new DashboardFrequenciaController();
        }

        [Fact(DisplayName = "UltimaConsolidacao deve retornar Ok com data")]
        public async Task UltimaConsolidacao_DeveRetornarOk()
        {
            var data = new DateTime(2024, 2, 15);

            _ultimaConsolidacaoUseCase
                .Setup(s => s.Executar(It.IsAny<int>()))
                .ReturnsAsync(data);

            var result = await _controller.UltimaConsolidacao(2024, _ultimaConsolidacaoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "UltimaConsolidacao deve retornar NoContent quando vazio")]
        public async Task UltimaConsolidacao_DeveRetornarNoContent()
        {
            _ultimaConsolidacaoUseCase
                .Setup(s => s.Executar(It.IsAny<int>()))
                .ReturnsAsync((DateTime?)null);

            var result = await _controller.UltimaConsolidacao(2024, _ultimaConsolidacaoUseCase.Object);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "ModalidadesPorAno deve retornar Ok com lista")]
        public async Task ModalidadesPorAno_DeveRetornarOk()
        {
            // Arrange
            var retorno = new List<RetornoModalidadesPorAnoDto>
    {
        new RetornoModalidadesPorAnoDto()
    };

            _modalidadesAnoUseCase
                .Setup(s => s.Executar(
                    It.IsAny<int>(),
                    It.IsAny<long>(),
                    It.IsAny<long>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .ReturnsAsync(retorno);

            // Act
            var result = await _controller.ModalidadesPorAno(
                anoLetivo: 2024,
                dreId: 1,
                ueId: 2,
                modalidade: 3,
                semestre: 1,
                useCase: _modalidadesAnoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<RetornoModalidadesPorAnoDto>>(ok.Value);
        }


        [Fact(DisplayName = "Listar deve retornar Ok com DTO")]
        public async Task Listar_DeveRetornarOk()
        {
            _dashAnoUseCase
                .Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Modalidade>(), It.IsAny<int>()))
                .ReturnsAsync(new List<GraficoFrequenciaGlobalPorAnoDto>
                {
                     new GraficoFrequenciaGlobalPorAnoDto()
                });

            var result = await _controller.Listar(2024, 1, 2, Modalidade.Fundamental, 1, _dashAnoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterFrequenciaGlobalPorDre deve retornar Ok com lista")]
        public async Task ObterFrequenciaGlobalPorDre_DeveRetornarOk()
        {
            var lista = new List<GraficoFrequenciaGlobalPorDREDto>()
            {
                new GraficoFrequenciaGlobalPorDREDto()
            };

            _freqPorDreUseCase.Setup(s => s.Executar(It.IsAny<FiltroGraficoFrequenciaGlobalPorDREDto>()))
                .ReturnsAsync(lista);

            var result = await _controller.ObterFrequenciaGlobalPorDre(new FiltroGraficoFrequenciaGlobalPorDREDto(), _freqPorDreUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoFrequenciaGlobalPorDREDto>>(ok.Value);
        }

        [Fact(DisplayName = "AusenciasPorMotivo deve retornar Ok com lista")]
        public async Task AusenciasPorMotivo_DeveRetornarOk()
        {
            var lista = new List<GraficoBaseDto>() { new GraficoBaseDto() };

            _ausenciasMotivoUseCase
                .Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Modalidade>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<int>()))
                .ReturnsAsync(lista);

            var result = await _controller.AusenciasPorMotivo(2024, 1, 2, Modalidade.Fundamental, "5", 10, 1, _ausenciasMotivoUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterAusenciasComJustificativa deve retornar Ok com lista")]
        public async Task ObterAusenciasComJustificativa_DeveRetornarOk()
        {
            var lista = new List<GraficoAusenciasComJustificativaResultadoDto>()
            {
                new GraficoAusenciasComJustificativaResultadoDto()
            };

            _ausJustUseCase
                .Setup(s => s.Executar(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Modalidade>(), It.IsAny<int>()))
                .ReturnsAsync(lista);

            var result = await _controller.ObterAusenciasComJustificativa(2024, 1, 2, Modalidade.Medio, 1, _ausJustUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoAusenciasComJustificativaResultadoDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterFrequenciasConsolidadacaoDiariaPorTurmaEAno deve retornar Ok com DTO")]
        public async Task ObterFreqDiaria_DeveRetornarOk()
        {
            _freqDiariaUseCase
                .Setup(s => s.Executar(It.IsAny<FrequenciasConsolidadacaoPorTurmaEAnoDto>()))
                .ReturnsAsync(new GraficoFrequenciaAlunoDto());

            var result = await _controller.ObterFrequenciasConsolidadacaoDiariaPorTurmaEAno(
                2024, 1, 2, 3, "A", 1, DateTime.Now, true, _freqDiariaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoFrequenciaAlunoDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterFrequenciasConsolidacaoSemanalMensalPorTurmaEAno deve retornar Ok com DTO")]
        public async Task ObterFreqSemanalMensal_DeveRetornarOk()
        {
            _freqSemanalMensalUseCase
                .Setup(s => s.Executar(
                    It.IsAny<FrequenciasConsolidadacaoPorTurmaEAnoDto>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<int?>(),
                    It.IsAny<TipoConsolidadoFrequencia>()))
                .ReturnsAsync(new GraficoFrequenciaAlunoDto());

            var result = await _controller.ObterFrequenciasConsolidacaoSemanalMensalPorTurmaEAno(
                2024, 1, 2, 3, TipoConsolidadoFrequencia.Semanal, "A", 1,
                DateTime.Now, DateTime.Now, 3, true, _freqSemanalMensalUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoFrequenciaAlunoDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterSemanasFiltro deve retornar Ok com lista")]
        public async Task ObterSemanasFiltro_DeveRetornarOk()
        {
            var retorno = new List<FiltroSemanaDto>
            {
                new FiltroSemanaDto()
            };

            _filtroSemanaUseCase
                .Setup(s => s.Executar(It.IsAny<int>()))
                .ReturnsAsync(retorno);

            var result = await _controller.ObterSemanasFiltro(2024, _filtroSemanaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<FiltroSemanaDto>>(ok.Value);
        }

        [Fact(DisplayName = "ConsolidarFrequenciasParaDashBorad deve retornar Ok")]
        public async Task ConsolidarFrequencias_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroConsolicacaoDiariaDashBoardFrequenciaDTO();

            _consolidacaoUseCase
                .Setup(s => s.Executar(It.IsAny<FiltroConsolicacaoDiariaDashBoardFrequenciaDTO>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ConsolidarFrequenciasParaDashBorad(
                filtro,
                _consolidacaoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkResult>(result);

            _consolidacaoUseCase.Verify(
                s => s.Executar(It.IsAny<FiltroConsolicacaoDiariaDashBoardFrequenciaDTO>()),
                Times.Once);
        }

    }
}
