using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers.DiarioBordo
{
    public class ObterAulaPorIdQueryHandlerTeste
    {
        [Fact]
        public async Task Handle_DeveRetornarAula_QuandoIdForValido()
        {
            // Arrange
            var aulaId = 1;
            var aulaEsperada = new Aula { Id = aulaId };

            var repositorioAulaMock = new Mock<IRepositorioAula>();
            repositorioAulaMock
                .Setup(r => r.ObterPorIdAsync(aulaId))
                .ReturnsAsync(aulaEsperada);

            var handler = new ObterAulaPorIdQueryHandler(repositorioAulaMock.Object);
            var query = new ObterAulaPorIdQuery(aulaId);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(aulaEsperada.Id, resultado.Id);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoAulaNaoForEncontrada()
        {
            // Arrange
            var aulaId = 999;

            var repositorioAulaMock = new Mock<IRepositorioAula>();
            repositorioAulaMock
                .Setup(r => r.ObterPorIdAsync(aulaId))
                .ReturnsAsync((Aula)null);

            var handler = new ObterAulaPorIdQueryHandler(repositorioAulaMock.Object);
            var query = new ObterAulaPorIdQuery(aulaId);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void Construtor_DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ObterAulaPorIdQueryHandler(null));
        }
    }
}
