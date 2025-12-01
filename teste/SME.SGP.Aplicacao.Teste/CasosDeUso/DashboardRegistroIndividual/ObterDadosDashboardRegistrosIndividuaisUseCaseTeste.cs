using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterDadosDashboardRegistrosIndividuaisUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardRegistrosIndividuaisUseCase _useCase;

        public ObterDadosDashboardRegistrosIndividuaisUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardRegistrosIndividuaisUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Contem_UeId_Deve_Chamar_Query_Por_Turma()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 10,
                Modalidade = Modalidade.Fundamental
            };

            var dadosRetorno = new List<GraficoBaseQuantidadeDoubleDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMediaRegistrosIndividuaisPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosRetorno);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterMediaRegistrosIndividuaisPorTurmaQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.DreId == filtro.DreId &&
                                     q.UeId == filtro.UeId && q.Modalidade == filtro.Modalidade),
                                 It.IsAny<CancellationToken>()),
                             Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterMediaRegistrosIndividuaisPorAnoQuery>(), It.IsAny<CancellationToken>()),
                             Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Nao_Contem_UeId_Deve_Chamar_Query_Por_Ano()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 0,
                Modalidade = Modalidade.EducacaoInfantil
            };

            var dadosRetorno = new List<GraficoBaseQuantidadeDoubleDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterMediaRegistrosIndividuaisPorAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosRetorno);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterMediaRegistrosIndividuaisPorAnoQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.DreId == filtro.DreId &&
                                     q.Modalidade == filtro.Modalidade),
                                 It.IsAny<CancellationToken>()),
                             Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterMediaRegistrosIndividuaisPorTurmaQuery>(), It.IsAny<CancellationToken>()),
                             Times.Never);
        }
    }
}
