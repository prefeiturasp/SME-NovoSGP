using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioQuestaoEncaminhamentoNAAPA : IRepositorioBase<QuestaoEncaminhamentoNAAPA>
    {
        Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId);
        Task<IEnumerable<RespostaQuestaoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId);
        Task<IEnumerable<PrioridadeEncaminhamentoNAAPADto>> ObterPrioridadeEncaminhamento();
    }
}
