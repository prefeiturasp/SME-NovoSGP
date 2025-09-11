using Bogus;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaAlunoDisciplinaPeriodoConsultaMock;
        private readonly ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandler _handler;

        public ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaAlunoDisciplinaPeriodoConsultaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandler(_repositorioFrequenciaAlunoDisciplinaPeriodoConsultaMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var codigoAluno = _faker.Random.String2(10);
            var bimestre = _faker.Random.Int(1, 4);
            var tipoFrequencia = _faker.PickRandom<TipoFrequenciaAluno>();
            var disciplinaId = _faker.Random.Int().ToString();
            var disciplinasId = new List<string> { disciplinaId }.ToArray();
            var turmaCodigo = _faker.Random.Int().ToString();
            var professor = _faker.Random.String2(10);
            var query = new ObterFrequenciaAlunoPorBimestreTurmaDisciplinaTipoQuery(codigoAluno, bimestre, tipoFrequencia, turmaCodigo, disciplinasId, professor);
            var frequenciaAluno = new FrequenciaAluno(
                codigoAluno, turmaCodigo, disciplinaId, _faker.Random.Long(), _faker.Date.Past(), _faker.Date.Future(), bimestre,
                _faker.Random.Int(), _faker.Random.Int(), _faker.Random.Int(), tipoFrequencia, _faker.Random.Int(),
                _faker.Random.Int(), _faker.Random.Int().ToString());
            _repositorioFrequenciaAlunoDisciplinaPeriodoConsultaMock
                .Setup(r => r.ObterPorAlunoBimestreAsync(codigoAluno, bimestre, tipoFrequencia, turmaCodigo, disciplinasId, professor))
                .ReturnsAsync(frequenciaAluno);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(codigoAluno, resultado.CodigoAluno);
            Assert.Equal(turmaCodigo, resultado.TurmaId);
            Assert.Equal(disciplinaId, resultado.DisciplinaId);
        }
    }
}
