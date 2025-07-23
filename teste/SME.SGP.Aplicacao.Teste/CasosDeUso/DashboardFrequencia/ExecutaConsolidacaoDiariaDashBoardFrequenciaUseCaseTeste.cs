using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase _useCase;

        public ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaConsolidacaoDiariaDashBoardFrequenciaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoNaoExistemCodigosUEs()
        {
            // Arrange
            var filtro = new FiltroConsolicacaoDiariaDashBoardFrequenciaDTO
            {
                AnoLetivo = 2025,
                Mes = 7
            };
            var mensagemRabbit = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosUEsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<string>)null);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterCodigosUEsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DevePublicarParaCadaUeERetornarTrue_QuandoExistemCodigosUEs()
        {
            // Arrange
            var filtro = new FiltroConsolicacaoDiariaDashBoardFrequenciaDTO
            {
                AnoLetivo = 2025,
                Mes = 7
            };
            var mensagemRabbit = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            var codigosUEs = new List<string> { "UE1", "UE2", "UE3" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosUEsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(codigosUEs);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterCodigosUEsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(codigosUEs.Count));
        }
    }
}
