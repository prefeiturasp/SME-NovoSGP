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
    public class DashboardAcompanhamentoAprendizagemControllerTests
    {
        private readonly Mock<IObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase> _obterUltimaConsolidacaoUseCase;
        private readonly Mock<IObterDashboardAcompanhamentoAprendizagemUseCase> _obterDashboardUseCase;
        private readonly Mock<IObterDashboardAcompanhamentoAprendizagemPorDreUseCase> _obterDashboardPorDreUseCase;

        private readonly DashboardAcompanhamentoAprendizagemController _controller;

        public DashboardAcompanhamentoAprendizagemControllerTests()
        {
            _obterUltimaConsolidacaoUseCase = new Mock<IObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase>();
            _obterDashboardUseCase = new Mock<IObterDashboardAcompanhamentoAprendizagemUseCase>();
            _obterDashboardPorDreUseCase = new Mock<IObterDashboardAcompanhamentoAprendizagemPorDreUseCase>();

            _controller = new DashboardAcompanhamentoAprendizagemController();
        }

        [Fact(DisplayName = "ObterUltimaConsolidacao deve retornar Ok com data")]
        public async Task ObterUltimaConsolidacao_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            DateTime? data = DateTime.Now;

            _obterUltimaConsolidacaoUseCase
                .Setup(x => x.Executar(anoLetivo))
                .ReturnsAsync(data);

            // Act
            var resultado = await _controller.ObterUltimaConsolidacao(
                anoLetivo,
                _obterUltimaConsolidacaoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsType<DateTime>(ok.Value);
        }

        [Fact(DisplayName = "ObterDashAcompanhamento deve retornar Ok com lista de gráficos")]
        public async Task ObterDashAcompanhamento_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroDashboardAcompanhamentoAprendizagemDto();

            var retorno = new List<GraficoBaseDto>
        {
            new GraficoBaseDto()
        };

            _obterDashboardUseCase
                .Setup(x => x.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterDashAcompanhamento(
                filtro,
                _obterDashboardUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<GraficoBaseDto>>(ok.Value);
            Assert.Single(lista);
        }


        [Fact(DisplayName = "ObterDashAcompanhamentoPorDre deve retornar Ok com lista de gráficos")]
        public async Task ObterDashAcompanhamentoPorDre_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroDashboardAcompanhamentoAprendizagemPorDreDto();

            var retorno = new List<GraficoBaseDto>
        {
            new GraficoBaseDto()
        };

            _obterDashboardPorDreUseCase
                .Setup(x => x.Executar(filtro))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _controller.ObterDashAcompanhamentoPorDre(
                filtro,
                _obterDashboardPorDreUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<GraficoBaseDto>>(ok.Value);
            Assert.Single(lista);
        }
    }
}