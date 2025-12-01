using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase _useCase;

        public ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterQuantidadeRegistrosIndividuaisPorAnoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Repassar_Filtro_Para_Query_E_Retornar_Resultado()
        {
            var filtro = new FiltroDasboardRegistroIndividualDTO
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 10,
                Modalidade = Modalidade.Fundamental
            };

            var dadosRetorno = new List<GraficoBaseDto> { new GraficoBaseDto { Descricao = "1A" } };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.DreId == filtro.DreId &&
                                     q.UeId == filtro.UeId && q.Modalidade == filtro.Modalidade),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dadosRetorno.First().Descricao, resultado.First().Descricao);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
