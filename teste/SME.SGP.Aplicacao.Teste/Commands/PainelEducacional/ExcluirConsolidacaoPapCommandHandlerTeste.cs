using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class ExcluirConsolidacaoPapCommandHandlerTeste
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap> _repositorioMock;
        private readonly ExcluirConsolidacaoPapCommandHandler _handler;

        public ExcluirConsolidacaoPapCommandHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap>();
            _handler = new ExcluirConsolidacaoPapCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public void Construtor_QuandoRepositorioNulo_DeveLancarArgumentNullException()
        {
            // Arrange & Act
            Action act = () => new ExcluirConsolidacaoPapCommandHandler(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("repositorioPainelEducacionalPap");
        }

        [Fact]
        public async Task Handle_QuandoComandoValido_DeveChamarRepositorioERetornarTrue()
        {
            // Arrange
            const int anoLetivo = 2025;
            var command = new ExcluirConsolidacaoPapCommand(anoLetivo);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();

            _repositorioMock.Verify(r => r.ExcluirConsolidacaoPorAno(anoLetivo), Times.Once);
        }
    }
}
