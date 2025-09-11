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
    public class ObterFrequenciaAlunoNaAulaQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioRegistroFrequenciaAlunoConsulta> _repositorioRegistroFrequenciaAlunoMock;
        private readonly ObterFrequenciaAlunoNaAulaQueryHandler _handler;

        public ObterFrequenciaAlunoNaAulaQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioRegistroFrequenciaAlunoMock = new Mock<IRepositorioRegistroFrequenciaAlunoConsulta>();
            _handler = new ObterFrequenciaAlunoNaAulaQueryHandler(_repositorioRegistroFrequenciaAlunoMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoRepositorioForNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFrequenciaAlunoNaAulaQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveChamarRepositorioComParametrosCorretos()
        {
            // Arrange
            var alunoCodigo = _faker.Random.String2(10);
            var aulaId = _faker.Random.Long();
            var query = new ObterFrequenciaAlunoNaAulaQuery(alunoCodigo, aulaId);
            var frequenciasEsperadas = new List<FrequenciaAlunoAulaDto>
            {
                new FrequenciaAlunoAulaDto { AlunoCodigo = alunoCodigo }
            };
            _repositorioRegistroFrequenciaAlunoMock
                .Setup(r => r.ObterFrequenciasDoAlunoNaAula(alunoCodigo, aulaId))
                .ReturnsAsync(frequenciasEsperadas);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(frequenciasEsperadas.Count, resultado.Count());
            Assert.Equal(frequenciasEsperadas.First().AlunoCodigo, resultado.First().AlunoCodigo);
        }
    }
}
