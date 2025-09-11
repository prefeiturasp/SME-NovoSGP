using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterFrequenciaAlunosGeralPorAnoQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioRegistroFrequenciaAlunoConsulta> _repositorioRegistroFrequenciaAlunoMock;
        private readonly ObterFrequenciaAlunosGeralPorAnoQueryHandler _handler;

        public ObterFrequenciaAlunosGeralPorAnoQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioRegistroFrequenciaAlunoMock = new Mock<IRepositorioRegistroFrequenciaAlunoConsulta>();
            _handler = new ObterFrequenciaAlunosGeralPorAnoQueryHandler(_repositorioRegistroFrequenciaAlunoMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFrequenciaAlunosGeralPorAnoQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var data = _faker.Date.Past();
            var ano = data.Year;
            var registrosFrequencia = new List<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>
            {
                new RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto
                {
                    AlunoCodigo = _faker.Random.Int().ToString(),
                    DisciplinaId = _faker.Random.Int().ToString(),
                    TurmaId = _faker.Random.Int(),
                    DataAula = data
                }
            };
            _repositorioRegistroFrequenciaAlunoMock
                .Setup(r => r.ObterFrequenciaAlunosGeralPorAnoQuery(ano))
                .ReturnsAsync(registrosFrequencia);
            var query = new ObterFrequenciaAlunosGeralPorAnoQuery(ano);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(data, resultado.First().DataAula);
        }
    }
}
