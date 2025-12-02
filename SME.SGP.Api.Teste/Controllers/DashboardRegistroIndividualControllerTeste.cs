using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardRegistroIndividualControllerTeste
    {
        private readonly Mock<IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase> _quantidadeAnoTurmaUseCase;
        private readonly Mock<IObterDadosDashboardRegistrosIndividuaisUseCase> _mediaUseCase;
        private readonly Mock<IObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase> _ultimaUseCase;
        private readonly Mock<IObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase> _semRegistroUseCase;
        private readonly Mock<IObterParametroDiasSemRegistroIndividualUseCase> _diasSemRegistroUseCase;
        private readonly Mock<IObterTotalRIsPorDreUseCase> _totalPorDreUseCase;

        private readonly DashboardRegistroIndividualController _controller;

        public DashboardRegistroIndividualControllerTeste()
        {
            _quantidadeAnoTurmaUseCase = new Mock<IObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase>();
            _mediaUseCase = new Mock<IObterDadosDashboardRegistrosIndividuaisUseCase>();
            _ultimaUseCase = new Mock<IObterUltimaConsolidacaoMediaRegistrosIndividuaisUseCase>();
            _semRegistroUseCase = new Mock<IObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase>();
            _diasSemRegistroUseCase = new Mock<IObterParametroDiasSemRegistroIndividualUseCase>();
            _totalPorDreUseCase = new Mock<IObterTotalRIsPorDreUseCase>();

            _controller = new DashboardRegistroIndividualController();
        }

        [Fact(DisplayName = "ObterQuantidadeRegistrosIndividuaisPorAnoTurma deve retornar Ok com lista")]
        public async Task ObterQuantidadeRegistrosIndividuaisPorAnoTurma_DeveRetornarOk()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO();
            var retorno = new List<GraficoTotalDiariosPendentesDTO> { new GraficoTotalDiariosPendentesDTO() };

            _quantidadeAnoTurmaUseCase.Setup(s => s.Executar(filtro))
                                      .ReturnsAsync(retorno);

            var result = await _controller.ObterQuantidadeRegistrosIndividuaisPorAnoTurma(
                filtro, _quantidadeAnoTurmaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoTotalDiariosPendentesDTO>>(ok.Value);
        }

        [Fact(DisplayName = "ObterDadosDashboard deve retornar Ok com lista")]
        public async Task ObterDadosDashboard_DeveRetornarOk()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO();
            var retorno = new List<GraficoBaseQuantidadeDoubleDto>
            {
                new GraficoBaseQuantidadeDoubleDto()
            };

            _mediaUseCase
                .Setup(s => s.Executar(filtro))
                .ReturnsAsync(retorno);

            var result = await _controller.ObterDadosDashboard(
                filtro,
                _mediaUseCase.Object
            );

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseQuantidadeDoubleDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterUltimaConsolidacao deve retornar Ok com DateTime quando houver valor")]
        public async Task ObterUltimaConsolidacao_DeveRetornarOk()
        {
            var data = DateTime.Now;

            _ultimaUseCase.Setup(s => s.Executar(2024))
                          .ReturnsAsync(data);

            var result = await _controller.ObterUltimaConsolidacao(2024, _ultimaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "ObterUltimaConsolidacao deve retornar Ok com null quando não há consolidação")]
        public async Task ObterUltimaConsolidacao_DeveRetornarOkComNull()
        {
            _ultimaUseCase.Setup(s => s.Executar(2024))
                          .ReturnsAsync((DateTime?)null);

            var result = await _controller.ObterUltimaConsolidacao(2024, _ultimaUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Null(ok.Value);
        }

        [Fact(DisplayName = "ObterDadosAlunosSemRegistro deve retornar Ok com Lista")]
        public async Task ObterDadosAlunosSemRegistro_DeveRetornarOk()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO();
            var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

            _semRegistroUseCase.Setup(s => s.Executar(filtro))
                               .ReturnsAsync(retorno);

            var result = await _controller.ObterDadosAlunosSemRegistro(filtro, _semRegistroUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeDiasSemRegistro deve retornar Ok com inteiro")]
        public async Task ObterQuantidadeDiasSemRegistro_DeveRetornarOk()
        {
            int retorno = 5;

            _diasSemRegistroUseCase
                .Setup(s => s.Executar(2024))
                .ReturnsAsync(retorno);

            var result = await _controller.ObterQuantidadeDiasSemRegistro(
                2024,
                _diasSemRegistroUseCase.Object
            );

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(ok.Value);
        }

        [Fact(DisplayName = "ObterTotalRIsPorDRE deve retornar Ok com Lista")]
        public async Task ObterTotalRIsPorDRE_DeveRetornarOk()
        {
            var filtro = new FiltroDashboardTotalRIsPorDreDTO();
            var retorno = new List<GraficoBaseDto> { new GraficoBaseDto() };

            _totalPorDreUseCase.Setup(s => s.Executar(filtro))
                               .ReturnsAsync(retorno);

            var result = await _controller.ObterTotalRIsPorDRE(filtro, _totalPorDreUseCase.Object);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<GraficoBaseDto>>(ok.Value);
        }
    }
}
