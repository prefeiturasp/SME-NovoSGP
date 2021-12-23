using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotificacao : IRepositorioBase<Notificacao>
    {
        Task ExcluirPorIdsAsync(long[] ids);
        Task ExcluirLogicamentePorIdsAsync(long[] ids);
        Task ExcluirPeloSistemaAsync(long[] ids);
    }
}