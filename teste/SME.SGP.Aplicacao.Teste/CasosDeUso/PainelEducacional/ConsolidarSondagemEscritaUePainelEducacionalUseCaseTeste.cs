using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoSondagemEscrita;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterSondagemEscrita;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarSondagemEscritaUePainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarSondagemEscritaUePainelEducacionalUseCase useCase;

        public ConsolidarSondagemEscritaUePainelEducacionalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarSondagemEscritaUePainelEducacionalUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_RetornarFalse()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSondagemEscritaUePorAnoLetivoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IEnumerable<SondagemEscritaUeDto>)null);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Quando_NaoExistirRegistros_Deve_RetornarFalse()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSondagemEscritaUePorAnoLetivoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<SondagemEscritaUeDto>());

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
        }

        [Fact]
        public async Task Executar_Quando_ExistirRegistros_Deve_SalvarERetornarTrue()
        {
            var dados = new List<SondagemEscritaUeDto>
            {
                new SondagemEscritaUeDto
                {
                    CodigoDre = "D1",
                    CodigoUe = "U1",
                    AnoLetivo = 2025,
                    SerieAno = 3,
                    Bimestre = 1,
                    PreSilabico = 1,
                    SilabicoSemValor = 2,
                    SilabicoComValor = 3,
                    SilabicoAlfabetico = 4,
                    Alfabetico = 5,
                    SemPreenchimento = 6,
                    QuantidadeAluno = 21
                },
                new SondagemEscritaUeDto
                {
                    CodigoDre = "D1",
                    CodigoUe = "U1",
                    AnoLetivo = 2025,
                    SerieAno = 3,
                    Bimestre = 1,
                    PreSilabico = 2,
                    SilabicoSemValor = 3,
                    SilabicoComValor = 4,
                    SilabicoAlfabetico = 5,
                    Alfabetico = 6,
                    SemPreenchimento = 7,
                    QuantidadeAluno = 27
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterSondagemEscritaUePorAnoLetivoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(dados);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            var captor = mediatorMock.Invocations
                .Where(x => x.Arguments.Any(a => a is SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand))
                .Select(x => x.Arguments[0] as SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand)
                .First();

            var consolidado = captor.Indicadores.First();
            Assert.Equal(3, consolidado.PreSilabico);
            Assert.Equal(5, consolidado.SilabicoSemValor);
            Assert.Equal(7, consolidado.SilabicoComValor);
            Assert.Equal(9, consolidado.SilabicoAlfabetico);
            Assert.Equal(11, consolidado.Alfabetico);
            Assert.Equal(13, consolidado.SemPreenchimento);
            Assert.Equal(48, consolidado.QuantidadeAluno);
            Assert.Equal(3, consolidado.SerieAno);
            Assert.Equal(2025, consolidado.AnoLetivo);
            Assert.Equal(1, consolidado.Bimestre);
        }
    }
}
