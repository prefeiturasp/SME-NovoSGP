using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.RemoveConexaoIdleUseCase
{
    public class RemoveConexaoIdleUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SME.SGP.Aplicacao.RemoveConexaoIdleUseCase useCase;

        public RemoveConexaoIdleUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SME.SGP.Aplicacao.RemoveConexaoIdleUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new SME.SGP.Aplicacao.RemoveConexaoIdleUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Comando_Corretamente()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<RemoveConexaoIdleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await useCase.Executar();

            mediatorMock.Verify(m => m.Send(
                It.IsAny<RemoveConexaoIdleCommand>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
