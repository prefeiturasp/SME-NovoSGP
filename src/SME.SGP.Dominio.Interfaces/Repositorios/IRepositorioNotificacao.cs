using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacao : IRepositorioBase<Notificacao>
    {
        Task ExcluirPorIdsAsync(long[] ids);
        Task ExcluirLogicamentePorIdsAsync(long[] ids);
        Task ExcluirPeloSistemaAsync(long[] ids);
        Task<IEnumerable<NotificacaoBasicaDto>> ObterNotificacoesPorAnoLetivoERfAsync(int anoLetivo, string usuarioRf, int limite = 5);
    }
}