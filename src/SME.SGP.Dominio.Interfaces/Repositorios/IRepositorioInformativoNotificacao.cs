using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativoNotificacao : IRepositorioBase<InformativoNotificacao>
    {
        Task<bool> RemoverLogicoPorInformeIdAsync(long informativoId);
        Task<IEnumerable<long>> ObterIdsNotificacoesPorInformeIdAsync(long informativoId);
    }
}
