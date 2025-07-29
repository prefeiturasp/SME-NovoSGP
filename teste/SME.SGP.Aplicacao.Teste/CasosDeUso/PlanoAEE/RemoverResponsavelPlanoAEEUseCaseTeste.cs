using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class RemoverResponsavelPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly RemoverResponsavelPlanoAEEUseCase useCase;

        public RemoverResponsavelPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new RemoverResponsavelPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_PlanoId_Correto()
        {
            // Arrange
            long planoId = 1;
            mediator.Setup(x => x.Send(It.IsAny<RemoverResponsavelPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            await useCase.Executar(planoId);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<RemoverResponsavelPlanoAEECommand>(c => c.PlanoAeeId == planoId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Remocao_Bem_Sucedida()
        {
            // Arrange
            long planoId = 2;
            mediator.Setup(x => x.Send(It.IsAny<RemoverResponsavelPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoId);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Remocao_Falhar()
        {
            // Arrange
            long planoId = 3;
            mediator.Setup(x => x.Send(It.IsAny<RemoverResponsavelPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(planoId);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Lanca_Excecao_Deve_Repassar()
        {
            // Arrange
            long planoId = 4;
            mediator.Setup(x => x.Send(It.IsAny<RemoverResponsavelPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(planoId));
        }
    }
}
