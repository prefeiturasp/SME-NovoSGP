using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoInicioPeriodoFechamentoUEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly NotificacaoInicioPeriodoFechamentoUEUseCase useCase;

        public NotificacaoInicioPeriodoFechamentoUEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new NotificacaoInicioPeriodoFechamentoUEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Executar_Teste()
        {
            var periodoFechamento = new PeriodoFechamentoBimestre(1, new PeriodoEscolar() { Id = 2 }, DateTime.Now, DateTime.Now.AddDays(10));
            var filtro = new FiltroFechamentoPeriodoAberturaDto(periodoFechamento, ModalidadeTipoCalendario.FundamentalMedio);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            mediator.Setup(x => x.Send(It.IsAny<ExecutaNotificacaoPeriodoFechamentoIniciandoCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediator.Verify(x => x.Send(It.IsAny<ExecutaNotificacaoPeriodoFechamentoIniciandoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
