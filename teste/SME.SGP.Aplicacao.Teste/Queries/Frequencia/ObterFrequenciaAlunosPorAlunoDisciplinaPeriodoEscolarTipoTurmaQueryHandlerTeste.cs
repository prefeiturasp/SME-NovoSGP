using Bogus;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaAlunoDisciplinaPeriodoMock;
        private readonly ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandler _handler;

        public ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQueryHandler(_repositorioFrequenciaAlunoDisciplinaPeriodoMock.Object);
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var codigoAluno = _faker.Random.Int(1, 1000).ToString();
            var disciplinaId = _faker.Random.Int(1, 100).ToString();
            var periodoEscolarId = _faker.Random.Int(1, 10);
            var tipo = _faker.PickRandom<TipoFrequenciaAluno>();
            var turmaId = _faker.Random.Int(1, 50).ToString();
            var frequenciaAluno = new FrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                DisciplinaId = disciplinaId,
                PeriodoEscolarId = periodoEscolarId,
                Tipo = tipo,
                TurmaId = turmaId
            };
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock
                .Setup(r => r.ObterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<TipoFrequenciaAluno>(), It.IsAny<string>()))
                .ReturnsAsync(frequenciaAluno);
            var query = new ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery(codigoAluno, disciplinaId, periodoEscolarId, tipo, turmaId);

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock.Verify(r => r.ObterAsync(codigoAluno, disciplinaId, periodoEscolarId, tipo, turmaId), Times.Once);
        }
    }
}
