using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using Xunit;
using System.Linq;

namespace SME.SGP.Tests.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAprovacaoUeUseCaseTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasAprovacaoUeUseCase useCase;

        public ConsultasAprovacaoUeUseCaseTests()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasAprovacaoUeUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar os dados de aprovação corretamente ao chamar o use case")]
        public async Task Deve_Obter_Aprovacao_Corretamente()
        {
            var anoLetivo = 2025;
            var codigoUe = "123456";
            var modalidade = "Fundamental";
            var numeroPagina = 1;
            var numeroRegistros = 10;

            var resultadoEsperado = new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>
            {
                Items = new List<PainelEducacionalAprovacaoUeDto>
                {
                    new PainelEducacionalAprovacaoUeDto
                    {
                        CodigoDre = "DRE01",
                        CodigoUe = codigoUe,
                        Turma = "5A",
                        Modalidade = modalidade,
                        TotalPromocoes = 20,
                        TotalRetencoesAusencias = 2,
                        TotalRetencoesNotas = 1,
                        AnoLetivo = anoLetivo
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalAprovacaoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.ObterAprovacao(anoLetivo, codigoUe, modalidade, numeroPagina, numeroRegistros);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(resultadoEsperado.TotalRegistros, resultado.TotalRegistros);
            Assert.Equal(codigoUe, resultado.Items.First().CodigoUe);
            Assert.Equal(modalidade, resultado.Items.First().Modalidade);

            mediatorMock.Verify(m =>
                m.Send(It.Is<PainelEducacionalAprovacaoUeQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoUe == codigoUe &&
                    q.Modalidade == modalidade &&
                    q.NumeroPagina == numeroPagina &&
                    q.NumeroRegistros == numeroRegistros),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
