using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioNotificacaoPlanoAEEObservacao : IRepositorioBase<NotificacaoPlanoAEEObservacao>
    {
        Task<IEnumerable<NotificacaoPlanoAEEObservacao>> ObterPorObservacaoPlanoAEEId(long observacaoPlanoId);
    }
}
