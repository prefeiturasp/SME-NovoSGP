using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
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
