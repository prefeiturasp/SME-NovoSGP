using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroColetivoAnexo : IRepositorioBase<RegistroColetivoAnexo>
    {
        Task<bool> RemoverPorRegistroColetivoId(long registroColetivoId);
    }
}
