using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesIndicadoresPap;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterDificuldadesIndicadoresPapQueryHandlerTeste
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap> _repositorioMock;
        private readonly ObterDificuldadesIndicadoresPapQueryHandler _handler;

        public ObterDificuldadesIndicadoresPapQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap>();
            _handler = new ObterDificuldadesIndicadoresPapQueryHandler(_repositorioMock.Object);
        }

        [Fact]
        public void Construtor_QuandoRepositorioNulo_DeveLancarArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ObterDificuldadesIndicadoresPapQueryHandler(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("repositorioConsolidacaoIndicadoresPap");
        }

        [Fact]
        public async Task Handle_QuandoChamado_DeveRepassarParaRepositorioERetornarResultado()
        {
            // Arrange
            var dadosMatriculaMock = new Faker<DadosMatriculaAlunoTipoPapDto>().Generate(5);
            var query = new ObterDificuldadesIndicadoresPapQuery(dadosMatriculaMock);

            var retornoEsperado = new Faker<ContagemDificuldadeIndicadoresPapPorTipoDto>().Generate(10);

            _repositorioMock
                .Setup(r => r.ObterContagemDificuldadesConsolidadaGeral(dadosMatriculaMock, It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().BeEquivalentTo(retornoEsperado);
            _repositorioMock.Verify(r => r.ObterContagemDificuldadesConsolidadaGeral(dadosMatriculaMock, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
