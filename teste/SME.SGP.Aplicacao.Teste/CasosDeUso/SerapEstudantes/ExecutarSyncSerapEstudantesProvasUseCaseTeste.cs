using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SerapEstudantes
{
    public class ExecutarSyncSerapEstudantesProvasUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExecutarSyncSerapEstudantesProvasUseCase useCase;

        public ExecutarSyncSerapEstudantesProvasUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExecutarSyncSerapEstudantesProvasUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new ExecutarSyncSerapEstudantesProvasUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Comando_E_Retornar_Resultado()
        {
            var param = new MensagemRabbit();
            var resultadoEsperado = true;

            mediatorMock.Setup(m => m.Send(
                It.Is<PublicarFilaSerapEstudantesCommand>(cmd =>
                    cmd.Fila == RotasRabbitSerapEstudantes.FilaProvaSync &&
                    (string)cmd.Mensagem == string.Empty),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(param);

            Assert.Equal(resultadoEsperado, resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSerapEstudantesCommand>(cmd =>
                    cmd.Fila == RotasRabbitSerapEstudantes.FilaProvaSync &&
                    (string)cmd.Mensagem == string.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
