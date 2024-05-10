using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoFimPeriodoFechamentoUEUseCase : AbstractUseCase, INotificacaoFimPeriodoFechamentoUEUseCase
    {
        public NotificacaoFimPeriodoFechamentoUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroFechamentoPeriodoAberturaDto>();

            await mediator.Send(new ExecutaNotificacaoPeriodoFechamentoEncerrandoCommand(filtro.PeriodoFechamentoBimestre, filtro.ModalidadeTipoCalendario));

            return true;
        }
    }
}