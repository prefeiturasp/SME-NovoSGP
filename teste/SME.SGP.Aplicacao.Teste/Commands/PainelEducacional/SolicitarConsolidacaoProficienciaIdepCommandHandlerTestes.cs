using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdep;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SolicitarConsolidacaoProficienciaIdepCommandHandlerTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SolicitarConsolidacaoProficienciaIdepCommandHandler _handler;

        public SolicitarConsolidacaoProficienciaIdepCommandHandlerTestes()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new SolicitarConsolidacaoProficienciaIdepCommandHandler(_mediatorMock.Object);
        }

        [Fact]
        public async Task DadoUmAnoLetivo_QuandoExecutarHandle_EntaoDevePublicarMensagemNaFilaCorretamente()
        {
            // Arrange
            var anoLetivo = 2023;
            var command = new SolicitarConsolidacaoProficienciaIdepCommand(anoLetivo);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                .ReturnsAsync(true);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdepPainelEducacional
            ), default), Times.Once);
        }
    }
}