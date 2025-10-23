using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.RabbitDeadletter
{
    public class RabbitDeadletterSerapSyncUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RabbitDeadletterSerapSyncUseCase useCase;

        public RabbitDeadletterSerapSyncUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RabbitDeadletterSerapSyncUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new RabbitDeadletterSerapSyncUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Comando_E_Retornar_Resultado()
        {
            var resultadoEsperado = true;

            mediatorMock.Setup(m => m.Send(
                It.Is<PublicarFilaSerapEstudantesCommand>(cmd =>
                    cmd.Fila == RotasRabbitSerapEstudantes.FilaDeadletterSync &&
                    (string)cmd.Mensagem == string.Empty),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar();

            Assert.Equal(resultadoEsperado, resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSerapEstudantesCommand>(cmd =>
                    cmd.Fila == RotasRabbitSerapEstudantes.FilaDeadletterSync &&
                    (string)cmd.Mensagem == string.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
