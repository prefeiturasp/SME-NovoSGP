using Bogus;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaAlunoDisciplinaPeriodoMock;
        private readonly ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandler _handler;

        public ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandler(_repositorioFrequenciaAlunoDisciplinaPeriodoMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var codigoAluno = _faker.Random.String2(10);
            var tipoFrequencia = _faker.PickRandom<TipoFrequenciaAluno>();
            var disciplinaId = _faker.Random.Int().ToString();
            var disciplinasId = new List<string> { disciplinaId }.ToArray();
            var turmaCodigo = _faker.Random.Int().ToString();
            var turmasCodigo = new List<string> { turmaCodigo }.ToArray();
            var bimestre = _faker.Random.Int(1, 4);
            var bimestres = new List<int> { bimestre }.ToArray();
            var periodoEscolar = _faker.Random.Long();
            var periodosEscolaresId = new List<long> { periodoEscolar }.ToArray();
            var query = new ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery(codigoAluno, tipoFrequencia, disciplinasId, turmasCodigo, bimestres, periodosEscolaresId);
            var frequenciaAluno = new FrequenciaAluno(
                codigoAluno, turmaCodigo, disciplinaId, periodoEscolar, _faker.Date.Past(), _faker.Date.Future(), bimestre, _faker.Random.Int(),
                _faker.Random.Int(), _faker.Random.Int(), tipoFrequencia, _faker.Random.Int(), _faker.Random.Int(), _faker.Random.Int().ToString());

            var frequenciasEsperadas = new List<FrequenciaAluno>
            {
                frequenciaAluno
            };
            _repositorioFrequenciaAlunoDisciplinaPeriodoMock
                .Setup(r => r.ObterPorAlunoTurmasDisciplinasDataAsync(codigoAluno, tipoFrequencia, disciplinasId, turmasCodigo, bimestres, periodosEscolaresId))
                .ReturnsAsync(frequenciasEsperadas);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(frequenciasEsperadas.Count, resultado.Count());
            Assert.Equal(frequenciasEsperadas.First().TurmaId, resultado.First().TurmaId);
        }
    }
}
