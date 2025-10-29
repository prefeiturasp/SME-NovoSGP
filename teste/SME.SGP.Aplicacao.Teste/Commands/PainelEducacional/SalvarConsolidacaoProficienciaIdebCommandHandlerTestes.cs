using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdeb;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarConsolidacaoProficienciaIdebCommandHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdeb> _repositorioMock;
        private readonly SalvarConsolidacaoProficienciaIdebCommandHandler _handler;

        public SalvarConsolidacaoProficienciaIdebCommandHandlerTestes()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdeb>();
            _handler = new SalvarConsolidacaoProficienciaIdebCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task DadoListaConsolidacaoIdebUeVazia_QuandoExecutarHandler_DeveRetornarFalse()
        {
            // Arrange
            var command = new SalvarConsolidacaoProficienciaIdebCommand(null);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task DadoListaConsolidacaoIdebUeValida_QuandoExecutarHandler_DeveChamarMetodosRepositorioEDevolverTrue()
        {
            // Arrange
            var anoLetivo = 2023;
            var consolidacaoIdebUe = new List<PainelEducacionalConsolidacaoProficienciaIdebUe>
            {
                new PainelEducacionalConsolidacaoProficienciaIdebUe { AnoLetivo = anoLetivo, CodigoUe = "UE1" },
                new PainelEducacionalConsolidacaoProficienciaIdebUe { AnoLetivo = anoLetivo, CodigoUe = "UE2" }
            };
            var command = new SalvarConsolidacaoProficienciaIdebCommand(consolidacaoIdebUe);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.True(resultado);
            _repositorioMock.Verify(r => r.LimparConsolidacaoPorAnoAsync(anoLetivo), Times.Once);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoAsync(It.IsAny<List<PainelEducacionalConsolidacaoProficienciaIdebUe>>()), Times.Once);
        }
    }
}
