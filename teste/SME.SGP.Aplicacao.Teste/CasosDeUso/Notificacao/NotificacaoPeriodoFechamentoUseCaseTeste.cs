using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoPeriodoFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly NotificacaoPeriodoFechamentoUseCase useCase;

        public NotificacaoPeriodoFechamentoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new NotificacaoPeriodoFechamentoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Teste_Teste()
        {
            var parametrosUe = new List<ParametrosSistema> { new ParametrosSistema { Valor = "1" } };
            var parametrosDre = new List<ParametrosSistema> { new ParametrosSistema { Valor = "1" } };
            var periodo = new PeriodoFechamentoBimestre(1, new PeriodoEscolar() { Id = 1 }, DateTime.Now, DateTime.Now.AddDays(5));

            mediator.Setup(x => x.Send(It.Is<ObterParametrosSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.DiasNotificacaoPeriodoFechamentoUe), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametrosUe);
            mediator.Setup(x => x.Send(It.Is<ObterParametrosSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.DiasNotificacaoPeriodoFechamentoDre), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametrosDre);
            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoBimestrePorDreUeEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);
            mediator.Setup(x => x.Send(It.IsAny<NotificarPeriodoFechamentoUeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(x => x.Send(It.IsAny<NotificarPeriodoFechamentoDreCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediator.Verify(x => x.Send(It.IsAny<ObterPeriodoFechamentoBimestrePorDreUeEDataQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
            mediator.Verify(x => x.Send(It.IsAny<NotificarPeriodoFechamentoUeCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediator.Verify(x => x.Send(It.IsAny<NotificarPeriodoFechamentoDreCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_Quando_Periodo_Nulo_Deve_Teste_Teste()
        {
            var parametros = new List<ParametrosSistema> { new ParametrosSistema { Valor = "1" } };

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);
            mediator.Setup(x => x.Send(It.IsAny<ObterPeriodoFechamentoBimestrePorDreUeEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoFechamentoBimestre)null);

            var resultado = await useCase.Executar(null);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<NotificarPeriodoFechamentoUeCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(x => x.Send(It.IsAny<NotificarPeriodoFechamentoDreCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
