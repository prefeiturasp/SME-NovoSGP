using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoUeFechamentosInsuficientesUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly NotificacaoUeFechamentosInsuficientesUseCase useCase;

        public NotificacaoUeFechamentosInsuficientesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new NotificacaoUeFechamentosInsuficientesUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Valor = "3" },
                new ParametrosSistema { Valor = "5" }
            };

            var parametroPercentual = new ParametrosSistema { Valor = "30" };

            var ue = new Ue { DreId = 1 };
            var periodoFechamento = new SME.SGP.Dominio.PeriodoFechamento { Ue = ue };
            var periodo1 = new PeriodoFechamentoBimestre { PeriodoFechamento = periodoFechamento };
            var periodo2 = new PeriodoFechamentoBimestre { PeriodoFechamento = periodoFechamento };

            var periodos = new List<PeriodoFechamentoBimestre> { periodo1, periodo2 };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroPercentual);

            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodos);

            mediator.Setup(x => x.Send(It.IsAny<ExecutaNotificacaoUeFechamentoInsuficientesCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);

            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
            mediator.Verify(x => x.Send(It.IsAny<ExecutaNotificacaoUeFechamentoInsuficientesCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Percentual_Nulo_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Valor = "3" }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediator.Setup(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ParametrosSistema)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(null));

            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(x => x.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
