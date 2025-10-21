using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotasParaConsolidacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterNotasParaConsolidacaoQueryHandlerTestes
    {
        private readonly Mock<IRepositorioPainelEducacionalConsolidacaoNotaConsulta> _repositorioPainelEducacionalConsolidacaoNotaConsultaMock;
        private readonly ObterNotasParaConsolidacaoQueryHandler _handler;

        public ObterNotasParaConsolidacaoQueryHandlerTestes()
        {
            _repositorioPainelEducacionalConsolidacaoNotaConsultaMock = new Mock<IRepositorioPainelEducacionalConsolidacaoNotaConsulta>();
            _handler = new ObterNotasParaConsolidacaoQueryHandler(_repositorioPainelEducacionalConsolidacaoNotaConsultaMock.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarDadosBrutosParaConsolidacao()
        {
            // Arrange
            short anoLetivo = 2023;
            var query = new ObterNotasParaConsolidacaoQuery(anoLetivo);
            var dadosBrutosEsperados = new List<PainelEducacionalConsolidacaoNotaDadosBrutos>
            {
                new PainelEducacionalConsolidacaoNotaDadosBrutos 
                {
                    AnoLetivo = anoLetivo,
                    TurmaNome = "Turma A",
                    Bimestre = 1,
                    CodigoDre = "DRE001",
                    CodigoUe = "UE001",
                    ConceitoDeAprovado = true,
                    IdComponenteCurricular = 101,
                    Modalidade = Modalidade.Fundamental,
                    Nota = 85,
                    AnoTurma = '5',
                    ValorConceito = "A",
                    ValorMedioNota = 50
                }
            };
            _repositorioPainelEducacionalConsolidacaoNotaConsultaMock
                .Setup(r => r.ObterDadosBrutosPorAnoLetivoAsync(anoLetivo))
                .ReturnsAsync(dadosBrutosEsperados);
            // Act
            var resultado = await _handler.Handle(query, default);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dadosBrutosEsperados.Count, resultado.Count());
            _repositorioPainelEducacionalConsolidacaoNotaConsultaMock.Verify(r => r.ObterDadosBrutosPorAnoLetivoAsync(anoLetivo), Times.Once);
        }
    }
}
