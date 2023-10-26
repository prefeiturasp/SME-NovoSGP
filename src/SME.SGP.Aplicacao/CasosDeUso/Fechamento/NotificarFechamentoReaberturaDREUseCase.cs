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
    public class NotificarFechamentoReaberturaDREUseCase : AbstractUseCase, INotificarFechamentoReaberturaDREUseCase
    {
        public NotificarFechamentoReaberturaDREUseCase(IMediator mediator) : base(mediator)
        {}
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificacaoFechamentoReaberturaDREDto>();

            if (filtro.EhNulo())
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }

            var adminsSgpDre = await mediator.Send(new ObterAdministradoresPorUEQuery(filtro.Dre));
            if (adminsSgpDre.NaoEhNulo() && adminsSgpDre.Any())
            {
                foreach (var adminSgpUe in adminsSgpDre)
                {
                    filtro.FechamentoReabertura.CodigoRf = adminSgpUe;
                    await mediator.Send(new ExecutaNotificacaoCadastroFechamentoReaberturaCommand(filtro.FechamentoReabertura));
                }
                    
            }

            foreach (var ue in filtro.Ues)
            {
                filtro.FechamentoReabertura.UeCodigo = ue;
                filtro.FechamentoReabertura.DreCodigo = filtro.Dre;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaUE, new FiltroNotificacaoFechamentoReaberturaUEDto(filtro.FechamentoReabertura), new Guid(), null));
            }                
                    
            return true;
        }
    }
}
