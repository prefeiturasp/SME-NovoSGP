using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterAulaPossuiFrequenciaQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaConsulta> _repositorioFrequenciaMock;
        private readonly ObterAulaPossuiFrequenciaQueryHandler _handler;

        public ObterAulaPossuiFrequenciaQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            _handler = new ObterAulaPossuiFrequenciaQueryHandler(_repositorioFrequenciaMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterAulaPossuiFrequenciaQueryHandler(null));
        }

        [Fact]
        public async Task DeveChamarRepositorio_E_RetornarFrequenciaAulaRegistrada()
        {
            // Arrange
            var aulaId = _faker.Random.Long(1);
            var query = new ObterAulaPossuiFrequenciaQuery(aulaId);
            var retornoEsperado = _faker.Random.Bool();
            _repositorioFrequenciaMock
                .Setup(r => r.FrequenciaAulaRegistrada(aulaId))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(retornoEsperado, resultado);
            _repositorioFrequenciaMock.Verify(r => r.FrequenciaAulaRegistrada(aulaId), Times.Once);
        }
    }
}
