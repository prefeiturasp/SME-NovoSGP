using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoTipoEscola
    {
        Task<long> SalvarAsync(ComunicadoTipoEscola comunicadoTipoEscola);
        Task<bool> ExcluirPorIdComunicado(long id);
        Task<IEnumerable<int>> ObterTiposEscolasPorComunicadoId(long id);
    }
}