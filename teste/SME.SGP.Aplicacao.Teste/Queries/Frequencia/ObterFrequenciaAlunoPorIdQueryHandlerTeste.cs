using Bogus;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunoPorIdQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo> _repositorioFrequenciaAlunoDisciplinaPeriodoMock;
        private readonly ObterFrequenciaAlunoPorIdQueryHandler _handler;

        public ObterFrequenciaAlunoPorIdQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodo>();
            _handler = new ObterFrequenciaAlunoPorIdQueryHandler(_repositorioFrequenciaAlunoDisciplinaPeriodoMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFrequenciaAlunoPorIdQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var frequenciaAlunoId = _faker.Random.Long();
            var frequenciaAluno = new FrequenciaAluno(
                _faker.Random.String2(10), _faker.Random.String2(10), _faker.Random.String2(10), _faker.Random.Long(),
                _faker.Date.Past(), _faker.Date.Future(), _faker.Random.Int(1, 4),
                _faker.Random.Int(), _faker.Random.Int(), _faker.Random.Int(),
                _faker.PickRandom<TipoFrequenciaAluno>(), _faker.Random.Int(),
                _faker.Random.Int(), _faker.Random.Int().ToString())
            { Id = frequenciaAlunoId };
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<long>()))
                .ReturnsAsync(frequenciaAluno);
            var query = new ObterFrequenciaAlunoPorIdQuery(frequenciaAlunoId);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(frequenciaAlunoId, resultado.Id);
        }
    }
}
