using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ExecutaConsolidacaoFrequenciaPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutaConsolidacaoFrequenciaPorAnoUseCase _useCase;

        public ExecutaConsolidacaoFrequenciaPorAnoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaConsolidacaoFrequenciaPorAnoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarComandosCorretamente()
        {
            int anoConsolidacao = 2024;
            var dataReferenciaEsperada = new DateTime(anoConsolidacao, 1, 1);

            _mediatorMock.Setup(m => m.Send(It.IsAny<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExecutarConsolidacaoFrequenciaNoAnoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            await _useCase.Executar(anoConsolidacao);

            _mediatorMock.Verify(m => m.Send(It.Is<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>(
                cmd => cmd.Ano == anoConsolidacao),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ExecutarConsolidacaoFrequenciaNoAnoCommand>(
                cmd => cmd.Data == dataReferenciaEsperada),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}