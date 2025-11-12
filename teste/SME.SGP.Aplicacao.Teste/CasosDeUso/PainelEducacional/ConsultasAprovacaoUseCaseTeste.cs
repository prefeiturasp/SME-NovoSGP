using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Teste.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAprovacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasAprovacaoUseCase useCase;

        public ConsultasAprovacaoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasAprovacaoUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar corretamente os dados de aprovação por DRE")]
        public async Task ObterAprovacao_DeveRetornarDadosCorretos()
        {
            // Arrange
            int anoLetivo = 2025;
            string codigoDre = "DRE01";

            var esperado = new List<PainelEducacionalAprovacaoDto>
            {
                new PainelEducacionalAprovacaoDto
                {
                    Modalidade = "Fundamental",
                    Indicadores = new List<IndicadorAprovacaoDto>
                    {
                        new IndicadorAprovacaoDto
                        {
                            SerieAno = "5º Ano",
                            TotalPromocoes = 20,
                            TotalRetencoesAusencias = 2,
                            TotalRetencoesNotas = 1
                        }
                    }
                }
            };

            //Act
            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalAprovacaoQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<PainelEducacionalAprovacaoDto>>(esperado));

            var resultado = await useCase.ObterAprovacao(anoLetivo, codigoDre);
            //Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Fundamental", resultado.First().Modalidade);
            Assert.Equal("5º Ano", resultado.First().Indicadores.First().SerieAno);
        }
    }
}
