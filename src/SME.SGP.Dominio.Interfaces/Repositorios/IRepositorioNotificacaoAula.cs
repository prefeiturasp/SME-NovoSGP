using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoAula
    {
        Task Inserir(long notificacaoId, long aulaId);
        Task<IEnumerable<NotificacaoAula>> ObterPorAulaAsync(long aulaId);
        Task Excluir(NotificacaoAula notificacaoAula);
    }
}
