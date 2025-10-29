using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdep;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarConsolidacaoProficienciaIdepCommandHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdep> _repositorioMock;
        private readonly SalvarConsolidacaoProficienciaIdepCommandHandler _handler;

        public SalvarConsolidacaoProficienciaIdepCommandHandlerTestes()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdep>();
            _handler = new SalvarConsolidacaoProficienciaIdepCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task DadoListaConsolidacaoIdepUeVazia_QuandoExecutarHandler_DeveRetornarFalse()
        {
            // Arrange
            var command = new SalvarConsolidacaoProficienciaIdepCommand(null);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task DadoListaConsolidacaoIdepUeValida_QuandoExecutarHandler_DeveChamarMetodosRepositorioEDevolverTrue()
        {
            // Arrange
            var anoLetivo = 2023;
            var consolidacaoIdepUe = new List<PainelEducacionalConsolidacaoProficienciaIdepUe>
            {
                new PainelEducacionalConsolidacaoProficienciaIdepUe { AnoLetivo = anoLetivo, CodigoUe = "UE1" },
                new PainelEducacionalConsolidacaoProficienciaIdepUe { AnoLetivo = anoLetivo, CodigoUe = "UE2" }
            };
            var command = new SalvarConsolidacaoProficienciaIdepCommand(consolidacaoIdepUe);
            // Act
            var resultado = await _handler.Handle(command, default);
            // Assert
            Assert.True(resultado);
            _repositorioMock.Verify(r => r.LimparConsolidacaoPorAnoAsync(anoLetivo), Times.Once);
            _repositorioMock.Verify(r => r.SalvarConsolidacaoAsync(It.IsAny<List<PainelEducacionalConsolidacaoProficienciaIdepUe>>()), Times.Once);
        }
    }
}