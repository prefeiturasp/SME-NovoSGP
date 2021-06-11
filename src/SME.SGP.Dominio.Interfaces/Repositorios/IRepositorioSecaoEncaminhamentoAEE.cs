using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoEncaminhamentoAEE : IRepositorioBase<SecaoEncaminhamentoAEE>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long encaminhamentoAeeId = 0);
        Task<IEnumerable<SecaoEncaminhamentoAEE>> ObterSecoesEncaminhamentoPorEtapa(List<int> etapas, long encaminhamentoAeeId = 0);
    }
}
