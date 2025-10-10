using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.ConsolidacaoFrequenciaTurma.ObterFrequenciaPorLimitePercentual;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.ConsolidacaoFrequenciaTurma;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioConsolidacaoFrequenciaTurmaConsulta> _repositorioMock;
        private readonly ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandler _handler;

        public ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioConsolidacaoFrequenciaTurmaConsulta>();
            _handler = new ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoChamado_DeveInvocarRepositorioComParametrosCorretos()
        {
            // Arrange
            const int anoLetivo = 2025;
            var query = new ObterQuantidadeAbaixoFrequenciaPorTurmaQuery(anoLetivo);

            var retornoEsperado = new Faker<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>()
                .RuleFor(r => r.CodigoTurma, f => f.Random.Int(1, 1000))
                .RuleFor(r => r.QuantidadeAbaixoMinimoFrequencia, f => f.Random.Int(1, 10))
                .Generate(5);

            _repositorioMock
                .Setup(r => r.ObterQuantitativoAlunosFrequenciaBaixaPorTurma(anoLetivo, TipoTurma.Programa))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEquivalentTo(retornoEsperado);

            _repositorioMock.Verify(r =>
                r.ObterQuantitativoAlunosFrequenciaBaixaPorTurma(
                    It.Is<int>(a => a == anoLetivo),
                    It.Is<TipoTurma>(t => t == TipoTurma.Programa)),
                Times.Once);
        }
    }
}
