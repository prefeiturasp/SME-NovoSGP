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

            if (filtro.FechamentoReabertura.EhParaUe())
            {
                var fechamentoReabertura = filtro.FechamentoReabertura;
                await mediator.Send(new ExecutaNotificacaoFechamentoReaberturaCommand(fechamentoReabertura, fechamentoReabertura.Ue.CodigoUe, null));
            }
            else
            {
                var verificarUesTipoCalendario = await mediator.Send(new ObterUEsComDREsPorModalidadeTipoCalendarioQuery(filtro.FechamentoReabertura.TipoCalendario.Modalidade, filtro.FechamentoReabertura.TipoCalendario.AnoLetivo));
                var agrupamentoUeporDre = verificarUesTipoCalendario.GroupBy(d => d.Dre).ToDictionary(group => group.Key, group => group.ToList().Select(s => s.CodigoUe));

                foreach (var valores in agrupamentoUeporDre)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaDRE, new FiltroNotificacaoFechamentoReaberturaDREDto(valores.Key.CodigoDre, valores.Value, filtro.FechamentoReabertura), new System.Guid(), filtro.Usuario));
            }

            return true;
        }        
    }
}
