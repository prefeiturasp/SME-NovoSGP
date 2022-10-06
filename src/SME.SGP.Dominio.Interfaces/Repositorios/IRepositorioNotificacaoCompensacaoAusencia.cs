using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacaoCompensacaoAusencia
    {
        void Inserir(long notificacaoId, long compensacaoAusenciaId);

        Task<IEnumerable<NotificacaoCompensacaoAusencia>> ObterPorCompensacao(long compensacaoAusenciaId);
        Task Excluir(NotificacaoCompensacaoAusencia notificacaoCompensacao);
    }
}
