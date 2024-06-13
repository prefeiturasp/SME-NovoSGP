using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioInformativoAnexo
    {
        Task<long> SalvarAsync(InformativoAnexo informativoAnexo);
        Task<IEnumerable<InformativoAnexoDto>> ObterAnexosPorInformativoIdAsync(long informativoId);
        Task<bool> RemoverLogicoPorInformativoIdAsync(long informativoId);
        Task<bool> RemoverPorInformativoIdAsync(long informativoId);
    }
}