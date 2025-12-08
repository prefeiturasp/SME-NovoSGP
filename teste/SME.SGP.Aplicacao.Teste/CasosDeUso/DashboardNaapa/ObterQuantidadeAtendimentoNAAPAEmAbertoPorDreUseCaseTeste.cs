using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardNaapa
{
    public class ObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase useCase;

        public ObterQuantidadeAtendimentoNAAPAEmAbertoPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_RetornarResultado_Teste()
        {
            var filtro = new FiltroQuantidadeAtendimentoNAAPAEmAbertoDto
            {
                AnoLetivo = 2025,
                DreId = 1,
                Modalidade = Modalidade.Fundamental
            };

            var esperado = new GraficoAtendimentoNAAPADto { TotaEncaminhamento = 5 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuantidadeAtendimentoNAAPAEmAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.Equal(esperado, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeAtendimentoNAAPAEmAbertoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Executar_Quando_MediatorNulo_Deve_Lancar_Excecao_Teste()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterQuantidadeEncaminhamentoNAAPAEmAbertoPorDreUseCase(null));
        }
    }
}
