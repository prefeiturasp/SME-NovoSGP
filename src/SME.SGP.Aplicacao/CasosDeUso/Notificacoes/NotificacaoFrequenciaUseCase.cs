using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class NotificacaoFrequenciaUseCase : AbstractUseCase, INotificacaoFrequencia
    {
        private readonly IServicoNotificacaoFrequencia servicoNotificacaoFrequencia;

        public NotificacaoFrequenciaUseCase(IMediator mediator, IServicoNotificacaoFrequencia servicoNotificacaoFrequencia) : base(mediator)
        {
            this.servicoNotificacaoFrequencia = servicoNotificacaoFrequencia;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registroFrequencia = mensagem.ObterObjetoMensagem<RegistroFrequencia>();
            await servicoNotificacaoFrequencia.VerificaRegraAlteracaoFrequencia(registroFrequencia.Id, registroFrequencia.CriadoEm, DateTime.Now);
            return true;
        }
    }
}
