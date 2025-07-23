using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ExcluirPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirPlanoAEEUseCase useCase;

        public ExcluirPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Plano_For_Excluido_Com_Sucesso()
        {
            // Arrange
            var planoAEEId = 1L;

            mediator.Setup(m => m.Send(It.Is<ExcluirPlanoAEECommand>(
                    c => c.PlanoAEEId == planoAEEId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(planoAEEId);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.Is<ExcluirPlanoAEECommand>(
                c => c.PlanoAEEId == planoAEEId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Plano_Nao_For_Excluido()
        {
            // Arrange
            var planoAEEId = 1L;

            mediator.Setup(m => m.Send(It.IsAny<ExcluirPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(planoAEEId);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Command()
        {
            // Arrange
            var planoAEEId = 1L;

            mediator.Setup(m => m.Send(It.IsAny<ExcluirPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao excluir plano"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(planoAEEId));
        }
    }
}
