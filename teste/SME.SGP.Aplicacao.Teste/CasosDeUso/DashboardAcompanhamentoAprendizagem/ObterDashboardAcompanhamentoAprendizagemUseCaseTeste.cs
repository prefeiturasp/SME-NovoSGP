using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAcompanhamentoAprendizagem
{
    public class ObterDashboardAcompanhamentoAprendizagemUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardAcompanhamentoAprendizagemUseCase _useCase;

        public ObterDashboardAcompanhamentoAprendizagemUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardAcompanhamentoAprendizagemUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Com_Dados_De_Turmas_Tratados_Corretamente()
        {
            var filtro = new FiltroDashboardAcompanhamentoAprendizagemDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 10,
                Semestre = 2
            };

            var acompanhamentosRetorno = new List<DashboardAcompanhamentoAprendizagemDto>
            {
                new DashboardAcompanhamentoAprendizagemDto
                {
                    Turma = "1A",
                    QuantidadeComAcompanhamento = 20,
                    QuantidadeSemAcompanhamento = 5
                },
                new DashboardAcompanhamentoAprendizagemDto
                {
                    Turma = "1B",
                    QuantidadeComAcompanhamento = 0,
                    QuantidadeSemAcompanhamento = 0
                }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDashBoardEncaminhamentoAprendizagemQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.DreId == filtro.DreId &&
                                     q.UeId == filtro.UeId && q.Semestre == filtro.Semestre),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(acompanhamentosRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            var itemComAcompanhamento = resultado.FirstOrDefault(r => r.Descricao == DashboardConstants.QuantidadeComAcompanhamento);
            Assert.NotNull(itemComAcompanhamento);
            Assert.Equal("1A", itemComAcompanhamento.Grupo);
            Assert.Equal(20, itemComAcompanhamento.Quantidade);

            var itemSemAcompanhamento = resultado.FirstOrDefault(r => r.Descricao == DashboardConstants.QuantidadeSemAcompanhamento);
            Assert.NotNull(itemSemAcompanhamento);
            Assert.Equal("1A", itemSemAcompanhamento.Grupo);
            Assert.Equal(5, itemSemAcompanhamento.Quantidade);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDashBoardEncaminhamentoAprendizagemQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
