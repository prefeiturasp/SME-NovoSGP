using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdepParaConsolidacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterProficienciaIdepParaConsolidacaoQueryHandlerTestes
    {
        private readonly ObterProficienciaIdepParaConsolidacaoQueryHandler _obterProficienciaIdepParaConsolidacaoQueryHandler;
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas> _repositorioConsultasMock;

        public ObterProficienciaIdepParaConsolidacaoQueryHandlerTestes()
        {
            _repositorioConsultasMock = new Mock<IRepositorioPainelEducacionalConsolidacaoProficienciaIdepConsultas>();
            _obterProficienciaIdepParaConsolidacaoQueryHandler = new ObterProficienciaIdepParaConsolidacaoQueryHandler(_repositorioConsultasMock.Object);
        }

        [Fact]
        public async Task DadoConsolidacaoExistente_QuandoExecutarHandle_EntaoRetornaListaProficienciaIdep()
        {
            // Arrange
            var anoLetivo = 2023;
            var query = new ObterProficienciaIdepParaConsolidacaoQuery(anoLetivo);
            _repositorioConsultasMock
                .Setup(r => r.ObterDadosParaConsolidarPorAnoAsync(anoLetivo))
                .ReturnsAsync(new List<PainelEducacionalConsolidacaoProficienciaIdepUe>
                {
                    new PainelEducacionalConsolidacaoProficienciaIdepUe { AnoLetivo = anoLetivo, CodigoUe = "UE1" },
                    new PainelEducacionalConsolidacaoProficienciaIdepUe { AnoLetivo = anoLetivo, CodigoUe = "UE2"  }
                });
            // Act
            var resultado = await _obterProficienciaIdepParaConsolidacaoQueryHandler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }
    }
}