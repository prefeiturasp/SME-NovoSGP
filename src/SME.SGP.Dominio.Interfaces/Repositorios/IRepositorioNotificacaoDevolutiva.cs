using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoDevolutiva
    {

        Task<IEnumerable<NotificacaoDevolutiva>> ObterPorDevolutivaId(long devolutivaId);

        Task Excluir(NotificacaoDevolutiva notificacao);

        Task Salvar(NotificacaoDevolutiva notificacao);

    }
}