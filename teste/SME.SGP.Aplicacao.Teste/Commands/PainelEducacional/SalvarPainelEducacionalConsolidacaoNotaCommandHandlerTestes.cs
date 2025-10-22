using Moq;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PainelEducacional
{
    public class SalvarPainelEducacionalConsolidacaoNotaCommandHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoNota> _repositorioPainelEducacionalConsolidacaoNotaMock;
        private readonly SalvarPainelEducacionalConsolidacaoNotaCommandHandler _handler;

        public SalvarPainelEducacionalConsolidacaoNotaCommandHandlerTestes()
        {
            _repositorioPainelEducacionalConsolidacaoNotaMock = new Mock<IRepositorioPainelEducacionalConsolidacaoNota>();
            _handler = new SalvarPainelEducacionalConsolidacaoNotaCommandHandler(_repositorioPainelEducacionalConsolidacaoNotaMock.Object);
        }

        [Fact]
        public async Task DadoNotasConsolidadasDreExistentes_QuandoExecutarHandle_EntaoDeveChamarRepositorioCorretamenteERetornarVerdadeiro()
        {
            // Arrange
            var notasConsolidadasDre = new List<PainelEducacionalConsolidacaoNota>
            {
                new PainelEducacionalConsolidacaoNota { AnoLetivo = 2022 },
                new PainelEducacionalConsolidacaoNota { AnoLetivo = 2023 }
            };
            var notasConsolidadasUe = new List<PainelEducacionalConsolidacaoNotaUe>
            {
                new PainelEducacionalConsolidacaoNotaUe { AnoLetivo = 2022 },
                new PainelEducacionalConsolidacaoNotaUe { AnoLetivo = 2023 }
            };
            var command = new SalvarPainelEducacionalConsolidacaoNotaCommand(notasConsolidadasDre, notasConsolidadasUe);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.LimparConsolidacaoAsync(2022), Times.Once);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNota>>()), Times.Once);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoUeAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNotaUe>>()), Times.Once);
        }

        [Fact]
        public async Task DadoNotasConsolidadasDreNulas_QuandoExecutarHandle_EntaoDeveRetornarFalsoESemChamarRepositorio()
        {
            // Arrange
            var notasConsolidadasDre = (IList<PainelEducacionalConsolidacaoNota>)null;
            var notasConsolidadasUe = new List<PainelEducacionalConsolidacaoNotaUe>
            {
                new PainelEducacionalConsolidacaoNotaUe { AnoLetivo = 2022 }
            };
            var command = new SalvarPainelEducacionalConsolidacaoNotaCommand(notasConsolidadasDre, notasConsolidadasUe);
            // Act
            var result = await _handler.Handle(command, default);
            // Assert
            Assert.False(result);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.LimparConsolidacaoAsync(It.IsAny<int>()), Times.Never);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNota>>()), Times.Never);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoUeAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNotaUe>>()), Times.Never);
        }

        [Fact]
        public async Task DadoNotasConsolidadasDreVazias_QuandoExecutarHandle_EntaoDeveRetornarFalsoESemChamarRepositorio()
        {
            // Arrange
            var notasConsolidadasDre = new List<PainelEducacionalConsolidacaoNota>();
            var notasConsolidadasUe = new List<PainelEducacionalConsolidacaoNotaUe>
            {
                new PainelEducacionalConsolidacaoNotaUe { AnoLetivo = 2022 }
            };
            var command = new SalvarPainelEducacionalConsolidacaoNotaCommand(notasConsolidadasDre, notasConsolidadasUe);
            // Act
            var result = await _handler.Handle(command, default);
            // Assert
            Assert.False(result);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.LimparConsolidacaoAsync(It.IsAny<int>()), Times.Never);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNota>>()), Times.Never);
            _repositorioPainelEducacionalConsolidacaoNotaMock.Verify(r => r.SalvarConsolidacaoUeAsync(It.IsAny<IList<PainelEducacionalConsolidacaoNotaUe>>()), Times.Never);
        }
    }
}
