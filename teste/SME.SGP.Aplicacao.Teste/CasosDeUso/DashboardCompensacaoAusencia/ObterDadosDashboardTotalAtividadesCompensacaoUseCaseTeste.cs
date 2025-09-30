using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardCompensacaoAusencia
{
    public class ObterDadosDashboardTotalAtividadesCompensacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardTotalAtividadesCompensacaoUseCase _useCase;

        public ObterDadosDashboardTotalAtividadesCompensacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardTotalAtividadesCompensacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Dados_Deve_Mapear_E_Retornar_Dto()
        {
            var dadosQuery = new List<TotalCompensacaoAusenciaDto>
            {
                new TotalCompensacaoAusenciaDto { DescricaoAnoTurma = "1º Ano", ModalidadeCodigo = Modalidade.Fundamental, Quantidade = 10 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardTotalAtividadesCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosQuery);

            var resultado = await _useCase.Executar(2025, 1, 1, 1, 1, 1);

            Assert.NotNull(resultado);
            Assert.Single(resultado.DadosCompensacaoAusenciaDashboard);

            var itemResultado = resultado.DadosCompensacaoAusenciaDashboard.First();
            Assert.Equal("EF - 1º Ano", itemResultado.Descricao);
            Assert.Equal(10, itemResultado.Quantidade);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardTotalAtividadesCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Dados_Deve_Retornar_Default()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardTotalAtividadesCompensacaoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TotalCompensacaoAusenciaDto>());

            var resultado = await _useCase.Executar(2025, 1, 1, 1, 1, 1);

            Assert.Null(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardTotalAtividadesCompensacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
