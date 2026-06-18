using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEncaminhamentoAEESecao : IRepositorioBase<EncaminhamentoAEESecao>
    {
        Task<IEnumerable<long>> ObterIdsSecoesPorEncaminhamentoAEEId(long encaminhamentoAEEId);
    }
}
