using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase _useCase;

        public ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveEnviarComandoComIdCorretoERetornarTrue()
        {
            // Arrange
            var filtroId = new SME.SGP.Infra.FiltroIdDto(12345);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtroId));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand>(cmd => cmd.AulaId == filtroId.Id),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
