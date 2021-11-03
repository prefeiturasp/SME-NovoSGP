using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarFechamentoReaberturaUEUseCase : AbstractUseCase, INotificarFechamentoReaberturaUEUseCase
    {
        public NotificarFechamentoReaberturaUEUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificacaoFechamentoReaberturaUEDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }
            else
            {
                await mediator.Send(new ExecutaNotificacaoFechamentoReaberturaCommand(filtro.FechamentoReabertura));
                return true;
            }
        }
    }
}
