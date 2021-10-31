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
    public class NotificarFechamentoReaberturaSMEUseCase : AbstractUseCase, INotificarFechamentoReaberturaSMEUseCase
    {
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;
        private readonly IServicoNotificacao servicoNotificacao;
        public NotificarFechamentoReaberturaSMEUseCase(IMediator mediator, IServicoUsuario servicoUsuario,
                                                    IServicoEol servicoEOL, IRepositorioFechamentoReabertura repositorioFechamentoReabertura, IServicoNotificacao servicoNotificacao) : base(mediator)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificacaoFechamentoReaberturaSMEDto>();

            if (filtro == null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar as notificações, pois o fechamento reabertura não possui dados", LogNivel.Informacao, LogContexto.Fechamento));
                return false;
            }
            else
            {
                await mediator.Send(new ExecutaNotificacaoFechamentoReaberturaCommand(filtro.FechamentoReabertura, filtro.Ues, filtro.Dre));                
                return true;
            }
        }
    }
}
