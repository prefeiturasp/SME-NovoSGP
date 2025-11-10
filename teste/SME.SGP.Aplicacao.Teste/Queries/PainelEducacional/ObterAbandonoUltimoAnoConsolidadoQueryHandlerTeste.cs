using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandonoUltimoAnoConsolidado;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterAbandonoUltimoAnoConsolidadoQueryHandlerTeste
    {
        private Mock<IRepositorioPainelEducacionalConsolidacaoAbandono> _repositorioPainelEducacionalConsolidacaoAbandono;
        private ObterAbandonoUltimoAnoConsolidadoQueryHandler _queryHandler;
        public ObterAbandonoUltimoAnoConsolidadoQueryHandlerTeste()
        {
            _repositorioPainelEducacionalConsolidacaoAbandono = new Mock<IRepositorioPainelEducacionalConsolidacaoAbandono>();
            _queryHandler = new (_repositorioPainelEducacionalConsolidacaoAbandono.Object);
        }

        [Fact]
        public async Task DadoRepositorioRetornaNulo_QuandoExecutarQuery_EntaoRetornaZero()
        {
            // Arrange
            _repositorioPainelEducacionalConsolidacaoAbandono.Setup(r => r.ObterUltimoAnoConsolidadoAsync()).ReturnsAsync((int?)null);
            // Act
            var resultado = await _queryHandler.Handle(new ObterAbandonoUltimoAnoConsolidadoQuery(), default);
            // Assert
            resultado.Should().Be(0);
        }

        [Fact]
        public async Task DadoRepositorioRetornaAno_QuandoExecutarQuery_EntaoRetornaAno()
        {
            // Arrange
            var ano = 2022;
            _repositorioPainelEducacionalConsolidacaoAbandono.Setup(r => r.ObterUltimoAnoConsolidadoAsync()).ReturnsAsync(ano);
            // Act
            var resultado = await _queryHandler.Handle(new ObterAbandonoUltimoAnoConsolidadoQuery(), default);
            // Assert
            resultado.Should().Be(ano);
        }
    }
}
