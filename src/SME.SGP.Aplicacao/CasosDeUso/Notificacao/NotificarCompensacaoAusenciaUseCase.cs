using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarCompensacaoAusenciaUseCase : AbstractUseCase, INotificarCompensacaoAusencia
    {
        private readonly IServicoNotificacaoFrequencia servicoNotificacaoFrequencia;

        public NotificarCompensacaoAusenciaUseCase(IMediator mediator, IServicoNotificacaoFrequencia servicoNotificacaoFrequencia) : base(mediator)
        {
            this.servicoNotificacaoFrequencia = servicoNotificacaoFrequencia;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var data = param.ObterObjetoMensagem<CompensacaoAusencia>();
            await servicoNotificacaoFrequencia.NotificarCompensacaoAusencia(data.Id);
            return true;
        }

    }
}
