using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardInformacoesEscolares
{
    public class ObterDashboardInformacoesEscolaresPorMatriculaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardInformacoesEscolaresPorMatriculaUseCase _useCase;

        public ObterDashboardInformacoesEscolaresPorMatriculaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardInformacoesEscolaresPorMatriculaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Repassar_Filtro_Para_Query_E_Retornar_Resultado()
        {
            var filtro = new FiltroGraficoMatriculaDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 10,
                Modalidade = Modalidade.Fundamental,
                Anos = new[] { new AnoItinerarioPrograma() },
                Semestre = 1
            };

            var dadosGraficoRetorno = new List<GraficoBaseDto>
            {
                new GraficoBaseDto("1º Ano", 100, "Matrículas Ativas")
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDadosDashboardMatriculaQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.DreId == filtro.DreId &&
                                     q.UeId == filtro.UeId && q.Modalidade == filtro.Modalidade &&
                                     q.Anos == filtro.Anos && q.Semestre == filtro.Semestre),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosGraficoRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dadosGraficoRetorno.First().Grupo, resultado.First().Grupo);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardMatriculaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
