using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativo : IRepositorioBase<Informativo>
    {
        Task<Informativo> ObterInformes(long id);
        Task<bool> RemoverAsync(long id);
    }
}
