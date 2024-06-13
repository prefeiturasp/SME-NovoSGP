using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioMapeamentoEstudanteSecao : IRepositorioBase<MapeamentoEstudanteSecao>
    {
        Task<IEnumerable<long>> ObterIdsSecoesPorMapeamentoEstudanteId(long mapeamentoEstudanteId);
    }
}
