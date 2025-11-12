using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Infra.Dtos.PainelEducacional;
using Xunit;
using SME.SGP.Infra;

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

        [Fact(DisplayName = "Deve retornar os dados de aprovação corretamente com paginação")]
        public async Task ObterAprovacao_DeveRetornarDadosCorretosComPaginacao()
        {
            int anoLetivo = 2025;
            string codigoUe = "123456";
            int modalidadeId = 1;

            var registrosEsperados = new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>
            {
                Items = new List<PainelEducacionalAprovacaoUeDto>
                {
                    new PainelEducacionalAprovacaoUeDto
                    {
                        CodigoDre = "DRE01",
                        CodigoUe = codigoUe,
                        Turma = "5A",
                        Modalidade = "Fundamental",
                        TotalPromocoes = 20,
                        TotalRetencoesAusencias = 2,
                        TotalRetencoesNotas = 1,
                        AnoLetivo = anoLetivo
                    }
                },
                TotalRegistros = 1,
                TotalPaginas = 1
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalAprovacaoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosEsperados);

            var resultado = await useCase.ObterAprovacao(anoLetivo, codigoUe, modalidadeId);

            Assert.NotNull(resultado);
            Assert.Single(resultado.Items);
            Assert.Equal(registrosEsperados.TotalRegistros, resultado.TotalRegistros);
            Assert.Equal(codigoUe, resultado.Items.First().CodigoUe);
            Assert.Equal("Fundamental", resultado.Items.First().Modalidade);

            mediatorMock.Verify(m =>
                m.Send(It.Is<PainelEducacionalAprovacaoUeQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoUe == codigoUe &&
                    q.ModalidadeId == modalidadeId
                    ),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
