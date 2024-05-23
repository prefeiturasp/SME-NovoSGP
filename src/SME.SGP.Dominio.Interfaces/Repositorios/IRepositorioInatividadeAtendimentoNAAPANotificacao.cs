using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInatividadeAtendimentoNAAPANotificacao : IRepositorioBase<InatividadeAtendimentoNAAPANotificacao>
    {
        Task<bool> RemoverLogicoPorNAAPAIdAsync(long encaminhamentoNAAPAId);
        Task<IEnumerable<long>> ObterIdsNotificacoesPorNAAPAIdAsync(long encaminhamentoNAAPAId);
    }
}
