using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesPapUltimoAnoConsolidado;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterInformacoesPapUltimoAnoConsolidadoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap> _repositorioMock;
        private readonly ObterInformacoesPapUltimoAnoConsolidadoQueryHandler _handler;

        public ObterInformacoesPapUltimoAnoConsolidadoQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap>();
            _handler = new ObterInformacoesPapUltimoAnoConsolidadoQueryHandler(_repositorioMock.Object);
        }

        [Fact]
        public void Construtor_QuandoRepositorioNulo_DeveLancarArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ObterInformacoesPapUltimoAnoConsolidadoQueryHandler(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("repositorioPainelEducacionalPap");
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaAno_DeveRetornarMesmoAno()
        {
            // Arrange
            const int anoEsperado = 2024;
            var query = new ObterInformacoesPapUltimoAnoConsolidadoQuery();

            _repositorioMock.Setup(r => r.ObterUltimoAnoConsolidado())
                          .ReturnsAsync(anoEsperado);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().Be(anoEsperado);
            _repositorioMock.Verify(r => r.ObterUltimoAnoConsolidado(), Times.Once);
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaNulo_DeveRetornarZero()
        {
            // Arrange
            var query = new ObterInformacoesPapUltimoAnoConsolidadoQuery();

            _repositorioMock.Setup(r => r.ObterUltimoAnoConsolidado())
                          .ReturnsAsync((int?)null);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().Be(0);
            _repositorioMock.Verify(r => r.ObterUltimoAnoConsolidado(), Times.Once);
        }
    }
}
