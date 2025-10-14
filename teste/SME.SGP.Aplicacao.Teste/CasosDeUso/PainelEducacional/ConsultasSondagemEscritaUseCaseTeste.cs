using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterSondagemEscrita;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasSondagemEscritaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasSondagemEscritaUseCase consultasSondagemEscritaUseCase;

        public ConsultasSondagemEscritaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            consultasSondagemEscritaUseCase = new ConsultasSondagemEscritaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Obter_Sondagem_Escrita_Deve_Retornar_Lista_De_Sondagem_Escrita_Dto()
        {
            var codigoDre = "DRE123";
            var codigoUe = "UE456";
            var anoLetivo = 2025;
            var bimestre = 1;
            var serieAno = 3;

            var sondagemEscritaEsperada = new List<SondagemEscritaDto>
            {
                new SondagemEscritaDto
                {
                    CodigoDre = codigoDre,
                    CodigoUe = codigoUe,
                    AnoLetivo = anoLetivo,
                    Bimestre = bimestre,
                    SerieAno = serieAno,
                    PreSilabico = 5,
                    SilabicoSemValor = 10,
                    SilabicoComValor = 15,
                    SilabicoAlfabetico = 20,
                    Alfabetico = 25,
                    SemPreenchimento = 0,
                    QuantidadeAlunos = 75
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterSondagemEscritaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sondagemEscritaEsperada);

            var resultado = await consultasSondagemEscritaUseCase.ObterSondagemEscrita(codigoDre, codigoUe, anoLetivo, bimestre, serieAno);

            Assert.NotNull(resultado);
            Assert.Equal(sondagemEscritaEsperada, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterSondagemEscritaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
