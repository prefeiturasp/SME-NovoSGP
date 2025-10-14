using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarPainelEducacionalConsolidacaoAbandonoCommandHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoAbandono> _repositorioMock;
        private readonly SalvarPainelEducacionalConsolidacaoAbandonoCommandHandler _commandHandler;

        public SalvarPainelEducacionalConsolidacaoAbandonoCommandHandlerTestes()
        {
            _repositorioMock = new Mock<IRepositorioPainelEducacionalConsolidacaoAbandono>();
            _commandHandler = new SalvarPainelEducacionalConsolidacaoAbandonoCommandHandler(_repositorioMock.Object);
        }

        [Fact]
        public async Task DadoIndicadoresDreNulos_QuandoExecutar_EntaoDeveRetornarFalso()
        {
            // Arrange
            var command = new SalvarPainelEducacionalConsolidacaoAbandonoCommand(null, new List<ConsolidacaoAbandonoUeDto>());

            // Act
            var resultado = await _commandHandler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _repositorioMock.Verify(r => r.LimparConsolidacao(It.IsAny<int>()), Times.Never);
            _repositorioMock.Verify(r => r.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoAbandonoUe>>()), Times.Never);
            _repositorioMock.Verify(r => r.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoAbandono>>()), Times.Never);
        }

        [Fact]
        public async Task DadoIndicadoresDreVazios_QuandoExecutar_EntaoDeveRetornarFalso()
        {
            // Arrange
            var command = new SalvarPainelEducacionalConsolidacaoAbandonoCommand(new List<ConsolidacaoAbandonoDto>(), new List<ConsolidacaoAbandonoUeDto>());

            // Act
            var resultado = await _commandHandler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeFalse();
            _repositorioMock.Verify(r => r.LimparConsolidacao(It.IsAny<int>()), Times.Never);
            _repositorioMock.Verify(r => r.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoAbandonoUe>>()), Times.Never);
            _repositorioMock.Verify(r => r.BulkInsertAsync(It.IsAny<IEnumerable<PainelEducacionalConsolidacaoAbandono>>()), Times.Never);
        }

        [Fact]
        public async Task DadoIndicadoresValidos_QuandoExecutar_EntaoDeveChamarRepositorioCorretamenteERetornarVerdadeiro()
        {
            // Arrange
            var anoLetivoBase = DateTime.Now.Year;
            var indicadoresDre = new List<ConsolidacaoAbandonoDto>
            {
                new ConsolidacaoAbandonoDto { AnoLetivo = anoLetivoBase },
                new ConsolidacaoAbandonoDto { AnoLetivo = anoLetivoBase - 1 }, // Menor ano
                new ConsolidacaoAbandonoDto { AnoLetivo = anoLetivoBase }
            };

            var indicadoresUe = new Faker<ConsolidacaoAbandonoUeDto>("pt_BR")
                .RuleFor(p => p.AnoLetivo, f => anoLetivoBase)
                .RuleFor(p => p.CodigoDre, f => f.Random.AlphaNumeric(10))
                .RuleFor(p => p.CodigoUe, f => f.Random.AlphaNumeric(10))
                .RuleFor(p => p.QuantidadeDesistencias, f => f.Random.Number(1, 10))
                .Generate(5);

            var command = new SalvarPainelEducacionalConsolidacaoAbandonoCommand(indicadoresDre, indicadoresUe);
            var menorAnoLetivo = anoLetivoBase - 1;

            // Act
            var resultado = await _commandHandler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();

            _repositorioMock.Verify(r => r.LimparConsolidacao(menorAnoLetivo), Times.Once);

            _repositorioMock.Verify(r => r.BulkInsertAsync(It.Is<IEnumerable<PainelEducacionalConsolidacaoAbandonoUe>>(
                lista => lista.Count() == indicadoresUe.Count)), Times.Once);

            _repositorioMock.Verify(r => r.BulkInsertAsync(It.Is<IEnumerable<PainelEducacionalConsolidacaoAbandono>>(
                lista => lista.Count() == indicadoresDre.Count)), Times.Once);
        }
    }
}
