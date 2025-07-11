using Bogus;
using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandlerTeste
    {
        private readonly Faker _faker;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandler _handler;

        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandlerTeste()
        {
            _faker = new Faker();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandler(_repositorioFrequenciaMock.Object);
        }

        [Fact(DisplayName = "Deve lançar exceção quando o repositório for nulo")]
        public void DeveLancarExcecao_QuandoRepositorioNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandler(null));
        }

        [Fact(DisplayName = "Deve chamar o repositório e retornar o valor de existência de frequência")]
        public async Task DeveChamarRepositorio_E_RetornarExistenciaDeFrequencia()
        {
            // Arrange      
            var query = new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(
                _faker.Random.AlphaNumeric(5),
                new[] { _faker.Random.Long(1).ToString() },
                _faker.Random.Long(1)
            );

            var retornoEsperado = _faker.Random.Bool();
            _repositorioFrequenciaMock
                .Setup(r => r.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);
            _repositorioFrequenciaMock.Verify(r => r.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                query.CodigoTurma,
                query.ComponentesCurricularesId,
                query.PeriodoEscolarId, It.IsAny<string>()), Times.Once);
        }
    }
}
