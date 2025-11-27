using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAEE
{
    public class ObterEncaminhamentoAEESituacoesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterEncaminhamentoAEESituacoesUseCase _useCase;

        public ObterEncaminhamentoAEESituacoesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterEncaminhamentoAEESituacoesUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir ano zero pelo ano atual")]
        public async Task Executar_DeveSubstituirAnoZeroPorAnoAtual()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = 123,
                UeId = 456
            };

            var retornoEsperado = new DashboardAEEEncaminhamentosDto();

            _mediator
                .Setup(m => m.Send(
                    It.Is<ObterEncaminhamentoAEESituacoesQuery>(q =>
                        q.Ano == anoAtual &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<DashboardAEEEncaminhamentosDto>(resultado);

            // Garante que o mediator foi chamado com o ano atualizado
            _mediator.Verify(m => m.Send(
                It.Is<ObterEncaminhamentoAEESituacoesQuery>(q =>
                    q.Ano == anoAtual
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact(DisplayName = "Executar deve usar o ano informado quando não for zero")]
        public async Task Executar_DeveUsarAnoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreId = 10,
                UeId = 20
            };

            var retorno = new DashboardAEEEncaminhamentosDto();

            _mediator
                .Setup(m => m.Send(
                    It.Is<ObterEncaminhamentoAEESituacoesQuery>(q =>
                        q.Ano == filtro.AnoLetivo &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<DashboardAEEEncaminhamentosDto>(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterEncaminhamentoAEESituacoesQuery>(q =>
                    q.Ano == filtro.AnoLetivo
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
