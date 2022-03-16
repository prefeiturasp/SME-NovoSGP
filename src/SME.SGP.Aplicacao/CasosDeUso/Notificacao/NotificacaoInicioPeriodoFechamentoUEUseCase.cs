using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoInicioPeriodoFechamentoUEUseCase : AbstractUseCase, INotificacaoInicioPeriodoFechamentoUEUseCase
    {
        public NotificacaoInicioPeriodoFechamentoUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroFechamentoPeriodoAberturaDto>();

            await mediator.Send(new ExecutaNotificacaoPeriodoFechamentoIniciandoCommand(filtro.PeriodoFechamentoBimestre, filtro.ModalidadeTipoCalendario));

            return true;
        }
    }
}