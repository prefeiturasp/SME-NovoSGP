using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDiarioBordo
{
    public class ObterUltimaConsolidacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterUltimaConsolidacaoDiarioBordoUseCase useCase;

        public ObterUltimaConsolidacaoDiarioBordoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterUltimaConsolidacaoDiarioBordoUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar valor do parâmetro quando existir")]
        public async Task Deve_Retornar_Valor_Parametro_Quando_Existir()
        {
            var valorEsperado = "2024-07-14";
            var anoLetivo = 2024;

            mediatorMock.Setup(a => a.Send(
                It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                    q.Ano == anoLetivo &&
                    q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoDiariosBordo),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema() { Valor = valorEsperado });

            var resultado = await useCase.Executar(anoLetivo);

            Assert.Equal(valorEsperado, resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar string vazia quando parâmetro não existir")]
        public async Task Deve_Retornar_Vazio_Quando_Parametro_Nao_Existir()
        {
            var anoLetivo = 2024;

            mediatorMock.Setup(a => a.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.Equal(string.Empty, resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
