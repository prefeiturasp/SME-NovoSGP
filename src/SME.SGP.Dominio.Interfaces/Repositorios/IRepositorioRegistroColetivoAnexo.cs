using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroColetivoAnexo : IRepositorioBase<RegistroColetivoAnexo>
    {
        Task<IEnumerable<AnexoDto>> ObterAnexoPorRegistroColetivoId(long registroColetivoId);
        Task<bool> RemoverPorRegistroColetivoId(long registroColetivoId);
    }
}
