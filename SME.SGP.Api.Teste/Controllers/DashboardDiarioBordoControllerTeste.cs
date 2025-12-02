using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class DashboardDiarioBordoControllerTeste
    {
        private readonly DashboardDiarioBordoController _controller;
        private readonly Mock<IObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase> _preenchidosPendentesUseCase;
        private readonly Mock<IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase> _pendentesDreUseCase;
        private readonly Mock<IObterUltimaConsolidacaoDiarioBordoUseCase> _ultimaConsolidacaoUseCase;

        public DashboardDiarioBordoControllerTeste()
        {
            _preenchidosPendentesUseCase = new Mock<IObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaUseCase>();
            _pendentesDreUseCase = new Mock<IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase>();
            _ultimaConsolidacaoUseCase = new Mock<IObterUltimaConsolidacaoDiarioBordoUseCase>();

            _controller = new DashboardDiarioBordoController();
        }

        [Fact(DisplayName = "ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurma deve retornar Ok com DTO")]
        public async Task ObterQuantidadePreenchidosPendentes_DeveRetornarOk()
        {
            // Arrange
            var filtro = new FiltroDasboardDiarioBordoDto();
            var resultado = new List<GraficoTotalDiariosPreenchidosEPendentesDTO>
            {
                new GraficoTotalDiariosPreenchidosEPendentesDTO()
            };

            _preenchidosPendentesUseCase
                .Setup(u => u.Executar(filtro))
                .ReturnsAsync(resultado);

            // Act
            var response = await _controller.ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurma(
                filtro, _preenchidosPendentesUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(response);
            Assert.IsAssignableFrom<IEnumerable<GraficoTotalDiariosPreenchidosEPendentesDTO>>(ok.Value);
        }

        [Fact(DisplayName = "ObterQuantidadeTotalDeDiariosPendentesPorDre deve retornar Ok com DTO")]
        public async Task ObterQuantidadePendentesPorDre_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            string ano = "5A";

            var resultado = new List<GraficoTotalDiariosEDevolutivasPorDreDTO>
            {
                new GraficoTotalDiariosEDevolutivasPorDreDTO()
            };

            _pendentesDreUseCase
                .Setup(u => u.Executar(anoLetivo, ano))
                .ReturnsAsync(resultado);

            // Act
            var response = await _controller.ObterQuantidadeTotalDeDiariosPendentesPorDre(
                anoLetivo, ano, _pendentesDreUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(response);
            Assert.IsAssignableFrom<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>>(ok.Value);
        }

        [Fact(DisplayName = "ObterUltimaConsolidacao deve retornar Ok")]
        public async Task ObterUltimaConsolidacao_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            var retorno = "UltimaConsolidacao";

            _ultimaConsolidacaoUseCase
                .Setup(u => u.Executar(anoLetivo))
                .ReturnsAsync(retorno);

            // Act
            var response = await _controller.ObterUltimaConsolidacao(
                anoLetivo, _ultimaConsolidacaoUseCase.Object);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(response);
            Assert.IsType<string>(ok.Value);
        }
    }
}
