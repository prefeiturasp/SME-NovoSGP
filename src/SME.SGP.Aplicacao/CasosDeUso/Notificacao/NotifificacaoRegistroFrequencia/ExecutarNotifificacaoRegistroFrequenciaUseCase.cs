using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Notificacao.NotificacaoRegistroFrequencia;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Notificacao.NotifificacaoRegistroFrequencia
{
    public class ExecutarNotificacaoRegistroFrequenciaUseCase : IExecutarNotificacaoRegistroFrequenciaUseCase
    {
        private readonly IServicoNotificacaoFrequencia servico;

        public ExecutarNotificacaoRegistroFrequenciaUseCase(IServicoNotificacaoFrequencia servico)
        {
            this.servico = servico;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await servico.ExecutaNotificacaoRegistroFrequencia();
            return true;
        }
    }
}
