using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ObterFrequenciaOuPlanoNaRecorrenciaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoMediatorRetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            long aulaId = 123;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFrequenciaOuPlanoNaRecorrenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new ObterFrequenciaOuPlanoNaRecorrenciaUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(aulaId);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaOuPlanoNaRecorrenciaQuery>(q =>
                q.AulaId == aulaId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalse_QuandoMediatorRetornarFalse()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            long aulaId = 456;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFrequenciaOuPlanoNaRecorrenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var useCase = new ObterFrequenciaOuPlanoNaRecorrenciaUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(aulaId);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterFrequenciaOuPlanoNaRecorrenciaQuery>(q =>
                q.AulaId == aulaId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
