using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class TratarRegistroFrequenciaAlunoUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TratarRegistroFrequenciaAlunoUseCase _useCase;

        public TratarRegistroFrequenciaAlunoUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new TratarRegistroFrequenciaAlunoUseCase(_mediatorMock.Object);
        }

        [Theory]
        [InlineData("2024", 2024, new long[] { 1, 2, 3 })]
        [InlineData("2023", 2023, new long[] { 50 })]
        public async Task Executar_DevePublicarMensagensParaCadaDreQuandoAnoValido(string anoMensagem, int anoEsperado, long[] dresEsperadas)
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", anoMensagem, Guid.NewGuid(), "usuario");

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresEsperadas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);

            foreach (long dreId in dresEsperadas)
            {
                _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                    cmd.Rota == RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoUe &&
                    ((FiltroTratarRegistroFrequenciaDto)cmd.Filtros).AnoLetivo == anoEsperado && 
                    ((FiltroTratarRegistroFrequenciaDto)cmd.Filtros).DreId == dreId &&         
                    ((FiltroTratarRegistroFrequenciaDto)cmd.Filtros).UeId == null              
                ), It.IsAny<CancellationToken>()), Times.Once);
            }

            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoObterDresFalhar()
        {
            // Arrange
            var anoMensagem = "2025";
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", anoMensagem, Guid.NewGuid(), "usuario");

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter DREs"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(mensagemRabbit));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao obter DREs", ex.Message);

            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoPublicarFilaFalhar()
        {
            // Arrange
            var anoMensagem = "2025";
            var mensagemRabbit = new MensagemRabbit("AcaoTeste", anoMensagem, Guid.NewGuid(), "usuario");
            long[] dresEsperadas = { 1, 2 };

            _mediatorMock.Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresEsperadas);

            _mediatorMock.SetupSequence(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true) 
                .ThrowsAsync(new Exception("Erro ao publicar na fila")); 

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(mensagemRabbit));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao publicar na fila", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            _mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once); 
        }
    }
}