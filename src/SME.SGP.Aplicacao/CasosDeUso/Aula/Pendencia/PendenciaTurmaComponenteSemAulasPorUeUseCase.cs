using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaTurmaComponenteSemAulasPorUeUseCase : IPendenciaTurmaComponenteSemAulasPorUeUseCase
    {
        public Task<bool> Executar(MensagemRabbit param)
        {
            var dreUeDto = param.ObterObjetoMensagem<DreUeDto>();
        }
    }
}
