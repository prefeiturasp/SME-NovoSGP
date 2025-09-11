using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class CalcularFrequenciaGeralUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly CalcularFrequenciaGeralUseCase useCase;

        public CalcularFrequenciaGeralUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new CalcularFrequenciaGeralUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Command_Quando_Ano_Atual_For_Valido()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;
            var mensagem = new MensagemRabbit { Mensagem = anoAtual.ToString() };

            mediatorMock
                 .Setup(x => x.Send(It.IsAny<CalcularFrequenciaGeralCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<CalcularFrequenciaGeralCommand>(c => c.Ano == anoAtual), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Command_Quando_Ano_For_Diferente()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "1999" };

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<CalcularFrequenciaGeralCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Executar_Command_Quando_Mensagem_Nao_For_Numero()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "abc" };

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<CalcularFrequenciaGeralCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
