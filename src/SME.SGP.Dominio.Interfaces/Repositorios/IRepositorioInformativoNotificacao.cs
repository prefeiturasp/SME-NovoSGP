using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativoNotificacao : IRepositorioBase<InformativoNotificacao>
    {
        Task<bool> RemoverLogicoPorInformativoIdAsync(long informativoId);
        Task<IEnumerable<long>> ObterIdsNotificacoesPorInformativoIdAsync(long informativoId);
        Task<long> ObterIdInformativoPorNotificacaoIdAsync(long notificacaoId);
        Task<bool> VerificaSeExisteNotificacaoInformePorIdUsuarioRfAsync(long informativoId, string usuarioRf);
    }
}
