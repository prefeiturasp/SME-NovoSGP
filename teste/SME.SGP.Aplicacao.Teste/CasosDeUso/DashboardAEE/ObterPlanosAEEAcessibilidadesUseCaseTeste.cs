using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAEE
{
    public class ObterPlanosAEEAcessibilidadesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterPlanosAEEAcessibilidadesUseCase _useCase;

        public ObterPlanosAEEAcessibilidadesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterPlanosAEEAcessibilidadesUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir AnoLetivo = 0 pelo ano atual")]
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

            var retornoMediator = new List<AEEAcessibilidadeRetornoDto>();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanosAEEAcessibilidadesQuery>(q =>
                        q.Ano == anoAtual &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanosAEEAcessibilidadesQuery>(q => q.Ano == anoAtual),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact(DisplayName = "Executar deve usar AnoLetivo informado quando for diferente de zero")]
        public async Task Executar_DeveUsarAnoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2024,
                DreId = 5,
                UeId = 10
            };

            var retornoMediator = new List<AEEAcessibilidadeRetornoDto>();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanosAEEAcessibilidadesQuery>(q =>
                        q.Ano == 2024 &&
                        q.DreId == 5 &&
                        q.UeId == 10),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanosAEEAcessibilidadesQuery>(q => q.Ano == 2024),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
