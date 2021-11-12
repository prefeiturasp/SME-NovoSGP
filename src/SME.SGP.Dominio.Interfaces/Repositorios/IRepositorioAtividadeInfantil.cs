using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtividadeInfantil : IRepositorioBase<AtividadeInfantil>
    {
        Task<IEnumerable<AtividadeInfantilDto>> ObterPorAulaId(long aulaId);
    }
}
