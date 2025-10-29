using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdebParaConsolidacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterProficienciaIdebParaConsolidacaoQueryHandlerTestes
    {
        private readonly ObterProficienciaIdebParaConsolidacaoQueryHandler _obterProficienciaIdebParaConsolidacaoQueryHandler;
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas> _repositorioConsultasMock;

        public ObterProficienciaIdebParaConsolidacaoQueryHandlerTestes()
        {
            _repositorioConsultasMock = new Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdebConsultas>();
            _obterProficienciaIdebParaConsolidacaoQueryHandler = new ObterProficienciaIdebParaConsolidacaoQueryHandler(_repositorioConsultasMock.Object);
        }

        [Fact]
        public async Task DadoConsolidacaoExistente_QuandoExecutarHandle_EntaoRetornaListaProficienciaIdeb()
        {
            // Arrange
            var anoLetivo = 2023;
            var query = new ObterProficienciaIdebParaConsolidacaoQuery(anoLetivo);
            _repositorioConsultasMock
                .Setup(r => r.ObterDadosParaConsolidarPorAnoAsync(anoLetivo))
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdebUe>
                {
                    new PainelEducacionalConsolidacaoProficienciaIdebUe { AnoLetivo = anoLetivo, CodigoUe = "UE1" },
                    new PainelEducacionalConsolidacaoProficienciaIdebUe { AnoLetivo = anoLetivo, CodigoUe = "UE2"  }
                });
            // Act
            var resultado = await _obterProficienciaIdebParaConsolidacaoQueryHandler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }
    }
}