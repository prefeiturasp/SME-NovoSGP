using Moq;
using MediatR;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacao;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{ 
    public class ConsultasAprovacaoUseCaseTests
    {
        [Fact]
        public async Task ObterAprovacao_DeveRetornarListaDoMediator()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ConsultasAprovacaoUseCase(mediatorMock.Object);
            var resultadoEsperado = new List<PainelEducacionalAprovacaoDto>
    {
        new PainelEducacionalAprovacaoDto
        {
            CodigoDre = "12345",
            SerieAno = "1º Ano",
            Modalidade = "Ensino Fundamental",
            TotalPromocoes = 10,
            TotalRetencoesAusencias = 2,
            TotalRetencoesNotas = 1,
            AnoLetivo = 2025
        }
    };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalAprovacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.ObterAprovacao(2025, "12345");

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("Ensino Fundamental", ((List<PainelEducacionalAprovacaoDto>)resultado)[0].Modalidade);
            Assert.Equal(10, ((List<PainelEducacionalAprovacaoDto>)resultado)[0].TotalPromocoes);

            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalAprovacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}