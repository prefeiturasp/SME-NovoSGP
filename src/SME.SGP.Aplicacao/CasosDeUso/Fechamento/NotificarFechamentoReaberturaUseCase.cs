using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarFechamentoReaberturaUseCase : AbstractUseCase, INotificarFechamentoReaberturaUseCase
    {
        public NotificarFechamentoReaberturaUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroFechamentoReaberturaNotificacaoDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }

            if (filtro.EhParaUe)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaUE, new FiltroNotificacaoFechamentoReaberturaUEDto(filtro), new Guid(), null));
            else
            {
                var verificarUesTipoCalendario = await mediator.Send(new ObterUEsComDREsPorModalidadeTipoCalendarioQuery(filtro.Modalidades, filtro.AnoLetivo));
                var agrupamentoUeporDre = verificarUesTipoCalendario.GroupBy(d => d.Dre.CodigoDre).ToDictionary(group => group.Key, group => group.ToList().Select(s => s.CodigoUe));

                foreach (var valores in agrupamentoUeporDre)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaDRE, new FiltroNotificacaoFechamentoReaberturaDREDto(valores.Key, valores.Value, filtro), new Guid(), null));
            }

            return true;
        }        
    }
}
