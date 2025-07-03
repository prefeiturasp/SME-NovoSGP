using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandler _handler;
        private readonly Faker _faker;

        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandlerTeste()
        {
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandler(_repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o repositório for nulo")]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQueryHandler(null));
        }

        [Theory(DisplayName = "Deve chamar o repositório e retornar o valor de existência de frequência")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeveChamarRepositorio_E_RetornarExistenciaDeFrequencia(bool retornoEsperado)
        {
            // Arrange
            var query = new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery(
                _faker.Random.AlphaNumeric(5),
                new[] { _faker.Random.Long(1).ToString() },
                new[] { _faker.Random.Long(1) },
                _faker.Random.AlphaNumeric(7)
            );

            _repositorioFrequenciaMock
                .Setup(r => r.ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(
                    query.CodigoTurma,
                    query.ComponentesCurricularesId,
                    query.PeriodosEscolaresIds,
                    query.Professor))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositorioFrequenciaMock.Verify(r => r.ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(
                query.CodigoTurma,
                query.ComponentesCurricularesId,
                query.PeriodosEscolaresIds,
                query.Professor), Times.Once);

            Assert.Equal(retornoEsperado, resultado);
        }
    }
}
