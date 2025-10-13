using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Infra.Dtos.PainelEducacional;
using MediatR;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasAbandonoPainelEducacionalUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarDtoCorreto()
        {
            var mediatorMock = new Mock<IMediator>();
            var dto = new PainelEducacionalAbandonoUeDto
            {
                Modalidades = new System.Collections.Generic.List<PainelEducacionalAbandonoTurmaDto> {
                    new PainelEducacionalAbandonoTurmaDto { Turma = "Turma 1", QuantidadeDesistentes = 2 }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAbandonoUeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dto);

            var useCase = new ConsultasAbandonoPainelEducacionalUeUseCase(mediatorMock.Object);
            var result = await useCase.Executar(2025, "dre", "ue", "1", 1, 10);

            Assert.NotNull(result);
            Assert.Single(result.Modalidades);
            Assert.Equal(1, result.TotalPaginas);
            Assert.Equal(1, result.TotalRegistros);
            Assert.Equal("Turma 1", result.Modalidades[0].Turma);
        }
    }
}
