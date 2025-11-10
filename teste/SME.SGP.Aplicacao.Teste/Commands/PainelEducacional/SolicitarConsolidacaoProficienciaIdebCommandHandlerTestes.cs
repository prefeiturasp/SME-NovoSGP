using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SolicitarConsolidacaoProficienciaIdeb;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SolicitarConsolidacaoProficienciaIdebCommandHandlerTestes
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SolicitarConsolidacaoProficienciaIdebCommandHandler _handler;

        public SolicitarConsolidacaoProficienciaIdebCommandHandlerTestes()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new SolicitarConsolidacaoProficienciaIdebCommandHandler(_mediatorMock.Object);
        }

        [Fact]
        public async Task DadoUmAnoLetivo_QuandoExecutarHandle_EntaoDevePublicarMensagemNaFilaCorretamente()
        {
            // Arrange
            var anoLetivo = 2023;
            var command = new SolicitarConsolidacaoProficienciaIdebCommand(anoLetivo);
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                .ReturnsAsync(true);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(c =>
                c.Rota == RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdebPainelEducacional
            ), default), Times.Once);
        }
    }
}