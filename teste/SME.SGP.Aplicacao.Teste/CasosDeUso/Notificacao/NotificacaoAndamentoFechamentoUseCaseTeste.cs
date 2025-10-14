using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoAndamentoFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoAndamentoFechamentoUseCase useCase;

        public NotificacaoAndamentoFechamentoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoAndamentoFechamentoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_ParametroNaoExiste_Deve_RetornarFalse_Teste()
        {
            mediatorMock.Setup(x => x.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.False(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_ParametrosExistem_Deve_RetornarTrue_Teste()
        {
            var parametros = new List<ParametrosSistema>
            {
                new ParametrosSistema { Valor = "3" }
            };

            var periodos = new List<PeriodoFechamentoBimestre>
            {
                new PeriodoFechamentoBimestre { Id = 1, PeriodoEscolar = new PeriodoEscolar { Id = 10 } },
                new PeriodoFechamentoBimestre { Id = 2, PeriodoEscolar = new PeriodoEscolar { Id = 20 } }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<VerificaSeExisteParametroSistemaPorTipoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametros);

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodos);

            mediatorMock.Setup(x => x.Send(It.IsAny<ExecutaNotificacaoAndamentoFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterParametrosSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.IsAny<ExecutaNotificacaoAndamentoFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(4));
        }

        [Fact]
        public async Task VerificaPeriodosFechamentoEncerrando_Quando_Chamado_Deve_Executar_Teste()
        {
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PeriodoFechamentoBimestre> {
                    new PeriodoFechamentoBimestre(),
                    new PeriodoFechamentoBimestre() 
                  });

            mediatorMock.Setup(x => x.Send(It.IsAny<ExecutaNotificacaoAndamentoFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var metodo = typeof(NotificacaoAndamentoFechamentoUseCase)
                .GetMethod("VerificaPeriodosFechamentoEncerrando", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)metodo.Invoke(useCase, new object[] { ModalidadeTipoCalendario.FundamentalMedio, 2 });

            mediatorMock.Verify(x => x.Send(It.IsAny<ObterPeriodosFechamentoBimestrePorDataFinalQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<ExecutaNotificacaoAndamentoFechamentoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}
