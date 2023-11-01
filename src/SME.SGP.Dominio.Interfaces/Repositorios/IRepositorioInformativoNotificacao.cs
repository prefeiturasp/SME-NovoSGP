using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativoNotificacao : IRepositorioBase<InformativoNotificacao>
    {
        Task<bool> RemoverPorInformativoIdAsync(long informativoId);
        Task<IEnumerable<long>> ObterIdsNotificacoesPorInformativoIdAsync(long informativoId);
    }
}
