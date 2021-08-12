using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComunicadoAnoEscolar
    {
        Task<long> SalvarAsync(ComunicadoAnoEscolar comunicadoAnoEscolar);
        Task<bool> ExcluirPorIdComunicado(long id);
        Task<IEnumerable<string>> ObterAnosEscolaresPorComunicadoId(long id);
    }
}