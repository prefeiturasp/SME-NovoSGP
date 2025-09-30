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
    public class ObterDashboardAcompanhamentoAprendizagemPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardAcompanhamentoAprendizagemPorDreUseCase _useCase;

        public ObterDashboardAcompanhamentoAprendizagemPorDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardAcompanhamentoAprendizagemPorDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Com_Dados_Tratados_Corretamente()
        {
            var filtro = new FiltroDashboardAcompanhamentoAprendizagemPorDreDto { AnoLetivo = 2025, Semestre = 1 };

            var acompanhamentosRetorno = new List<DashboardAcompanhamentoAprendizagemPorDreDto>
            {
                new DashboardAcompanhamentoAprendizagemPorDreDto
                {
                    Dre = "DRE-01",
                    QuantidadeComAcompanhamento = 10,
                    QuantidadeSemAcompanhamento = 5
                },
                new DashboardAcompanhamentoAprendizagemPorDreDto
                {
                    Dre = "DRE-02",
                    QuantidadeComAcompanhamento = 0,
                    QuantidadeSemAcompanhamento = 0
                }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterDashboardAcompanhamentoAprendizagemPorDreQuery>(q =>
                                     q.AnoLetivo == filtro.AnoLetivo && q.Semestre == filtro.Semestre),
                                 It.IsAny<CancellationToken>()))
                         .ReturnsAsync(acompanhamentosRetorno);

            var resultado = await _useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            var itemComAcompanhamento = resultado.FirstOrDefault(r => r.Descricao == DashboardConstants.QuantidadeComAcompanhamento);
            Assert.NotNull(itemComAcompanhamento);
            Assert.Equal("DRE-01", itemComAcompanhamento.Grupo);
            Assert.Equal(10, itemComAcompanhamento.Quantidade);

            var itemSemAcompanhamento = resultado.FirstOrDefault(r => r.Descricao == DashboardConstants.QuantidadeSemAcompanhamento);
            Assert.NotNull(itemSemAcompanhamento);
            Assert.Equal("DRE-01", itemSemAcompanhamento.Grupo);
            Assert.Equal(5, itemSemAcompanhamento.Quantidade);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDashboardAcompanhamentoAprendizagemPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
