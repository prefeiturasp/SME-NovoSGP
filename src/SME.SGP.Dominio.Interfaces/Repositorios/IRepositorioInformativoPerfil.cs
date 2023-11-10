using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativoPerfil : IRepositorioBase<InformativoPerfil>
    {
        Task<bool> RemoverLogicoPorInformativoIdAsync(long informativoId);
    }
}
