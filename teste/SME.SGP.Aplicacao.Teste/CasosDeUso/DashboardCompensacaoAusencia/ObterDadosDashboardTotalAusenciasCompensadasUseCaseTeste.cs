using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardCompensacaoAusencia
{
    public class ObterDadosDashboardTotalAusenciasCompensadasUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardTotalAusenciasCompensadasUseCase _useCase;

        public ObterDadosDashboardTotalAusenciasCompensadasUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardTotalAusenciasCompensadasUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Dados_Deve_Mapear_E_Retornar_Dto_Corretamente()
        {
            var dadosDetalhados = new List<Infra.TotalCompensacaoAusenciaDto>
            {
                new Infra.TotalCompensacaoAusenciaDto { DescricaoAnoTurma = "1º Ano", ModalidadeCodigo = Modalidade.Fundamental, Quantidade = 15 }
            };

            var dadosTotais = new Infra.Dtos.TotalCompensacaoAusenciaDto { TotalAulas = 100, TotalCompensacoes = 15 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardTotalAusenciasCompensadasQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosDetalhados);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalCompensacaoAusenciaPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(dadosTotais);

            var resultado = await _useCase.Executar(2025, 1, 1, 1, 1, 1);

            Assert.NotNull(resultado);
            Assert.Equal(dadosTotais.TotalCompensacoesFormatado, resultado.TagTotalCompensacaoAusencia);
            Assert.Single(resultado.DadosCompensacaoAusenciaDashboard);

            var itemResultado = resultado.DadosCompensacaoAusenciaDashboard.First();
            Assert.Equal("EF - 1º Ano", itemResultado.Descricao);
            Assert.Equal(15, itemResultado.Quantidade);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardTotalAusenciasCompensadasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTotalCompensacaoAusenciaPorAnoLetivoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
