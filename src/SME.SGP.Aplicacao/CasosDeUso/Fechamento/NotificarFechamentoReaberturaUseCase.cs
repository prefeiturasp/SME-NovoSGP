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
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;
        public NotificarFechamentoReaberturaUseCase(IMediator mediator, IServicoUsuario servicoUsuario,
                                                    IServicoEol servicoEOL, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao) : base(mediator)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroFechamentoReaberturaNotificacaoDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }
            else
            {
                if (filtro.FechamentoReabertura.EhParaSme()) //todas as DRE's e UE's
                {
                    var verificarUesTipoCalendario = await mediator.Send(new ObterGestoresDreUePorTipoCalendarioModalidadeQuery(filtro.FechamentoReabertura.TipoCalendario.Modalidade, filtro.FechamentoReabertura.TipoCalendario.AnoLetivo));
                    var agrupamentoUeporDre = verificarUesTipoCalendario.GroupBy(d => d.Dre).ToDictionary(group => group.Key, group => group.ToList().Select(s => s.Ue));

                    foreach (var valores in agrupamentoUeporDre)
                       await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoFechamentoReaberturaSME, new FiltroNotificacaoFechamentoReaberturaSMEDto(valores.Key, valores.Value, filtro.FechamentoReabertura), new System.Guid(), filtro.Usuario));
                }
                else if (filtro.FechamentoReabertura.EhParaUe())
                {
                    var fechamentoReabertura = filtro.FechamentoReabertura;
                    var ues = new List<string>() { fechamentoReabertura.Ue.CodigoUe };

                    await mediator.Send(new ExecutaNotificacaoFechamentoReaberturaCommand(fechamentoReabertura, ues, null));
                }

                return true;
            }
        }

        
    }
}
