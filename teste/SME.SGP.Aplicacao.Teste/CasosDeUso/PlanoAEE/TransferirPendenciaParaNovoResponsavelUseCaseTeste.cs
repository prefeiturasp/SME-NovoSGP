using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class TransferirPendenciaParaNovoResponsavelUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly TransferirPendenciaParaNovoResponsavelUseCase useCase;

        public TransferirPendenciaParaNovoResponsavelUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new TransferirPendenciaParaNovoResponsavelUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Command_Da_Mensagem_E_Enviar_Para_Mediator()
        {
            // Arrange
            var command = new TransferirPendenciaParaNovoResponsavelCommand(1,2);
            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(command);

            mediator.Setup(x => x.Send(It.IsAny<TransferirPendenciaParaNovoResponsavelCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<TransferirPendenciaParaNovoResponsavelCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Se_Mediator_Retornar_False()
        {
            // Arrange
            var command = new TransferirPendenciaParaNovoResponsavelCommand(1,2);
            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(command);

            mediator.Setup(x => x.Send(It.IsAny<TransferirPendenciaParaNovoResponsavelCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Mensagem_For_Invalida()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = "json inválido";

            // Act & Assert
            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => useCase.Executar(mensagemRabbit));
        }
    }
}