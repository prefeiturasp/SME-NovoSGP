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
    public class DashboardInformacoesEscolaresControllerTeste
    {
        private readonly DashboardInformacoesEscolaresController _controller;
        private readonly Mock<IObterDashboardInformacoesEscolaresPorMatriculaUseCase> _matriculaUseCase;
        private readonly Mock<IObterDashboardInformacoesEscolaresPorTurmaUseCase> _turmaUseCase;
        private readonly Mock<IObterDataConsolidacaoInformacoesEscolaresUseCase> _ultimaConsolidacaoUseCase;
        private readonly Mock<IObterModalidadeAnoItineranciaProgramaUseCase> _modalidadeAnoUseCase;

        public DashboardInformacoesEscolaresControllerTeste()
        {
            _matriculaUseCase = new Mock<IObterDashboardInformacoesEscolaresPorMatriculaUseCase>();
            _turmaUseCase = new Mock<IObterDashboardInformacoesEscolaresPorTurmaUseCase>();
            _ultimaConsolidacaoUseCase = new Mock<IObterDataConsolidacaoInformacoesEscolaresUseCase>();
            _modalidadeAnoUseCase = new Mock<IObterModalidadeAnoItineranciaProgramaUseCase>();

            _controller = new DashboardInformacoesEscolaresController();
        }

        [Fact(DisplayName = "ObterGraficoPorMatricula deve retornar Ok com lista")]
        public async Task ObterGraficoPorMatricula_DeveRetornarOk()
        {
            var filtro = new FiltroGraficoMatriculaDto();

            var retorno = new List<GraficoBaseDto>
        {
            new GraficoBaseDto()
        };

            _matriculaUseCase.Setup(s => s.Executar(filtro))
                             .ReturnsAsync(retorno);

            var result = await _controller.ObterGraficoPorMatricula(
                filtro,
                _matriculaUseCase.Object
            );

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterGraficoPorTurma deve retornar Ok com lista")]
        public async Task ObterGraficoPorTurma_DeveRetornarOk()
        {
            var filtro = new FiltroGraficoMatriculaDto();

            var retorno = new List<GraficoBaseDto>
        {
            new GraficoBaseDto()
        };

            _turmaUseCase.Setup(s => s.Executar(filtro))
                         .ReturnsAsync(retorno);

            var result = await _controller.ObterGraficoPorTurma(
                filtro,
                _turmaUseCase.Object
            );

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }

        [Fact(DisplayName = "UltimaConsolidacao deve retornar Ok quando houver data")]
        public async Task UltimaConsolidacao_DeveRetornarOk()
        {
            int anoLetivo = 2024;
            var data = new DateTime(2024, 02, 15);

            _ultimaConsolidacaoUseCase.Setup(s => s.Executar(anoLetivo))
                .ReturnsAsync(data);

            var result = await _controller.UltimaConsolidacao(
                anoLetivo,
                _ultimaConsolidacaoUseCase.Object
            );

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "UltimaConsolidacao deve retornar NoContent quando não houver data")]
        public async Task UltimaConsolidacao_DeveRetornarNoContent()
        {
            int anoLetivo = 2024;

            _ultimaConsolidacaoUseCase.Setup(s => s.Executar(anoLetivo))
                .ReturnsAsync((DateTime?)null);

            var result = await _controller.UltimaConsolidacao(
                anoLetivo,
                _ultimaConsolidacaoUseCase.Object
            );

            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "ModalidadesPorAno deve retornar Ok com lista")]
        public async Task ModalidadesPorAno_DeveRetornarOk()
        {
            // Arrange
            var retorno = new List<RetornoModalidadesPorAnoItineranciaProgramaDto>
    {
        new RetornoModalidadesPorAnoItineranciaProgramaDto()
    };

            _modalidadeAnoUseCase
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
                useCase: _modalidadeAnoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<RetornoModalidadesPorAnoItineranciaProgramaDto>>(ok.Value);
        }

    }
}
