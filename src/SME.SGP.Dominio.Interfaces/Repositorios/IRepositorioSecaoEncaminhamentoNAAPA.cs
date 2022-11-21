using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSecaoEncaminhamentoNAAPA : IRepositorioBase<SecaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long encaminhamentoNAAPAId = 0);
        Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorEtapa(List<int> etapas, long encaminhamentoNAAPAId = 0);
    }
}
