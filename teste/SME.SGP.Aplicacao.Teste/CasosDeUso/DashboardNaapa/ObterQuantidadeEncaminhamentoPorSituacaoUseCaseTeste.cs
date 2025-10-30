using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterQuantidadeEncaminhamentoPorSituacaoUseCaseTeste
    {
        private readonly Mock<IRepositorioConsolidadoEncaminhamentoNAAPA> repositorioMock;
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterQuantidadeEncaminhamentoPorSituacaoUseCase useCase;

        public ObterQuantidadeEncaminhamentoPorSituacaoUseCaseTeste()
        {
            repositorioMock = new Mock<IRepositorioConsolidadoEncaminhamentoNAAPA>();
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterQuantidadeEncaminhamentoPorSituacaoUseCase(repositorioMock.Object, mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_RetornarResultado_Teste()
        {
            var filtro = new FiltroGraficoEncaminhamentoPorSituacaoDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                UeId = 2,
                Modalidade = Modalidade.Fundamental
            };

            var dadosRepositorio = new List<DadosGraficoSitaucaoPorUeAnoLetivoDto>
            {
                new DadosGraficoSitaucaoPorUeAnoLetivoDto { Quantidade = 5, Situacao = Dominio.Enumerados.SituacaoNAAPA.EmAtendimento },
                new DadosGraficoSitaucaoPorUeAnoLetivoDto { Quantidade = 3, Situacao = Dominio.Enumerados.SituacaoNAAPA.Encerrado }
            };

            repositorioMock.Setup(r => r.ObterDadosGraficoSituacaoPorUeAnoLetivo(filtro.AnoLetivo, filtro.UeId, filtro.DreId, (int?)filtro.Modalidade))
                           .ReturnsAsync(dadosRepositorio);

            var dataUltimaConsolidacao = DateTime.Today;
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDataUltimaConsolicacaoDashboardNaapaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(dataUltimaConsolidacao);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(dataUltimaConsolidacao, resultado.DataUltimaConsolidacao);
            Assert.Equal(2, resultado.Graficos.Count);
            Assert.Equal(5, resultado.Graficos[0].Quantidade);
            Assert.Equal("Em atendimento", resultado.Graficos[0].Descricao);
            Assert.Equal(3, resultado.Graficos[1].Quantidade);
            Assert.Equal("Encerrado", resultado.Graficos[1].Descricao);

            repositorioMock.Verify(r => r.ObterDadosGraficoSituacaoPorUeAnoLetivo(filtro.AnoLetivo, filtro.UeId, filtro.DreId, (int?)filtro.Modalidade), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDataUltimaConsolicacaoDashboardNaapaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_Quando_RepositorioNulo_Deve_LancarExcecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterQuantidadeEncaminhamentoPorSituacaoUseCase(null, mediatorMock.Object));
        }

        [Fact]
        public void Constructor_Quando_MediatorNulo_Deve_LancarExcecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterQuantidadeEncaminhamentoPorSituacaoUseCase(repositorioMock.Object, null));
        }
    }
}
