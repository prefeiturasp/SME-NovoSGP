using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Teste.Controllers
{
    public class DashboardCompensacaoAusenciaControllerTeste
    {
        private readonly DashboardCompensacaoAusenciaController _controller;

        private readonly Mock<IObterDadosDashboardTotalAusenciasCompensadasUseCase> _ausenciasCompensadasUseCase;
        private readonly Mock<IObterDadosDashboardTotalAtividadesCompensacaoUseCase> _atividadesCompensacaoUseCase;

        public DashboardCompensacaoAusenciaControllerTeste()
        {
            _ausenciasCompensadasUseCase = new Mock<IObterDadosDashboardTotalAusenciasCompensadasUseCase>();
            _atividadesCompensacaoUseCase = new Mock<IObterDadosDashboardTotalAtividadesCompensacaoUseCase>();

            _controller = new DashboardCompensacaoAusenciaController();
        }

        [Fact(DisplayName = "ObterTotalAusenciasCompensadas deve retornar Ok com DTO")]
        public async Task ObterTotalAusenciasCompensadas_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            long dreId = 1;
            long ueId = 2;
            int modalidade = 3;
            int bimestre = 1;
            int semestre = 2;

            var dto = new GraficoCompensacaoAusenciaDto();

            _ausenciasCompensadasUseCase
                .Setup(s => s.Executar(anoLetivo, dreId, ueId, modalidade, bimestre, semestre))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.ObterTotalAusenciasCompensadas(
                anoLetivo,
                dreId,
                ueId,
                modalidade,
                bimestre,
                semestre,
                _ausenciasCompensadasUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoCompensacaoAusenciaDto>(ok.Value);
        }

        [Fact(DisplayName = "ObterTotalCompensacoesConsideradas deve retornar Ok com DTO")]
        public async Task ObterTotalCompensacoesConsideradas_DeveRetornarOk()
        {
            // Arrange
            int anoLetivo = 2024;
            long dreId = 1;
            long ueId = 2;
            int modalidade = 3;
            int bimestre = 1;
            int semestre = 2;

            var dto = new GraficoCompensacaoAusenciaDto();

            _atividadesCompensacaoUseCase
                .Setup(s => s.Executar(anoLetivo, dreId, ueId, modalidade, bimestre, semestre))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.ObterTotalCompensacoesConsideradas(
                anoLetivo,
                dreId,
                ueId,
                modalidade,
                bimestre,
                semestre,
                _atividadesCompensacaoUseCase.Object
            );

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<GraficoCompensacaoAusenciaDto>(ok.Value);
        }
    }
}
