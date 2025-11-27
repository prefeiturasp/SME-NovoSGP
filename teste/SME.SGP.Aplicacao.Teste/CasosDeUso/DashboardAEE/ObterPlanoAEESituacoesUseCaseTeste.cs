using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAEE
{
    public class ObterPlanoAEESituacoesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterPlanoAEESituacoesUseCase _useCase;

        public ObterPlanoAEESituacoesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterPlanoAEESituacoesUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir Ano = 0 pelo ano atual")]
        public async Task Executar_DeveSubstituirAnoZeroPeloAnoAtual()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = 1,
                UeId = 2
            };

            var retorno = new DashboardAEEPlanosSituacaoDto();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanoAEESituacoesQuery>(q =>
                        q.Ano == anoAtual &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<DashboardAEEPlanosSituacaoDto>(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanoAEESituacoesQuery>(q =>
                    q.Ano == anoAtual
                ),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact(DisplayName = "Executar deve usar Ano informado quando != 0")]
        public async Task Executar_DeveUsarAnoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = 10,
                UeId = 20
            };

            var retorno = new DashboardAEEPlanosSituacaoDto();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanoAEESituacoesQuery>(q =>
                        q.Ano == DateTime.Now.Year &&
                        q.DreId == 10 &&
                        q.UeId == 20
                    ),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(retorno);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<DashboardAEEPlanosSituacaoDto>(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanoAEESituacoesQuery>(q =>
                    q.Ano == DateTime.Now.Year
                ),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
