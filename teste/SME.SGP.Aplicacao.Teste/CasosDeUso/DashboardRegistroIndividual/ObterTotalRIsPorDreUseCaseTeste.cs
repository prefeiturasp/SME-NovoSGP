using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterTotalRIsPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalRIsPorDreUseCase _useCase;

        public ObterTotalRIsPorDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalRIsPorDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Repassar_Filtro_Para_Query_E_Retornar_Resultado()
        {
            var filtro = new FiltroDashboardTotalRIsPorDreDTO
            {
                AnoLetivo = 2025,
                Ano = "1"
            };

            var dadosRetorno = new List<GraficoBaseDto> { new GraficoBaseDto { Descricao = "DRE-CL" } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTotalRIsPorDreQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.Ano == filtro.Ano),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dadosRetorno.First().Descricao, resultado.First().Descricao);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTotalRIsPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
