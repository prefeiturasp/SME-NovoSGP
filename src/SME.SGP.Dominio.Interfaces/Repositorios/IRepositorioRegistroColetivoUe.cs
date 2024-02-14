using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroColetivoUe : IRepositorioBase<RegistroColetivoUe>
    {
        Task<bool> RemoverPorRegistroColetivoId(long registroColetivoId);
    }
}
