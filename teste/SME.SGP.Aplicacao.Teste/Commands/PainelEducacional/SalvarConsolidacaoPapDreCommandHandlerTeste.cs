using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarConsolidacaoPapDreCommandHandlerTeste
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap> _repositorioMock;
        private readonly SalvarConsolidacaoPapDreCommandHandler _handler;

        public SalvarConsolidacaoPapDreCommandHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoIndicadoresPap>();
            _handler = new SalvarConsolidacaoPapDreCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task Handle_QuandoComandoValido_DeveChamarRepositorioComDadosCorretosERetornarTrue()
        {
            // Arrange
            var consolidacaoMock = new Faker<PainelEducacionalConsolidacaoPapDre>().Generate(3);
            var command = new SalvarConsolidacaoPapDreCommand(consolidacaoMock);

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();
            _repositorioMock.Verify(r => r.SalvarConsolidacaoDre(consolidacaoMock), Times.Once);
        }
    }
}