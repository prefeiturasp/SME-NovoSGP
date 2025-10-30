using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoFimPeriodoFechamentoUEUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoFimPeriodoFechamentoUEUseCase useCase;

        public NotificacaoFimPeriodoFechamentoUEUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoFimPeriodoFechamentoUEUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_MensagemValida_Deve_EnviarComandoERetornarTrue_Teste()
        {
            var filtro = new FiltroFechamentoPeriodoAberturaDto(
                new PeriodoFechamentoBimestre { Id = 1, PeriodoEscolar = new PeriodoEscolar { Id = 10 } },
                ModalidadeTipoCalendario.FundamentalMedio
            );

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            mediatorMock.Setup(x => x.Send(It.IsAny<ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
