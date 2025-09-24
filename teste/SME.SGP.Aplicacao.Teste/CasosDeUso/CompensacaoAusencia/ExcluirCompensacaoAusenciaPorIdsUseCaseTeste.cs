using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class ExcluirCompensacaoAusenciaPorIdsUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirCompensacaoAusenciaPorIdsUseCase _useCase;

        public ExcluirCompensacaoAusenciaPorIdsUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirCompensacaoAusenciaPorIdsUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveEnviarComandoComIdsCorretosERetornarTrue()
        {
            // Arrange
            var ids = new long[] { 10, 20, 30 };
            var filtro = new SME.SGP.Infra.FiltroCompensacaoAusenciaDto(ids);
            var mensagemRabbit = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<IRequest<Unit>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirCompensacaoAusenciaPorIdsCommand>(cmd =>
                    cmd.CompensacaoAusenciaIds != null &&
                    cmd.CompensacaoAusenciaIds.Length == ids.Length &&
                    cmd.CompensacaoAusenciaIds[0] == 10 &&
                    cmd.CompensacaoAusenciaIds[1] == 20 &&
                    cmd.CompensacaoAusenciaIds[2] == 30
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
