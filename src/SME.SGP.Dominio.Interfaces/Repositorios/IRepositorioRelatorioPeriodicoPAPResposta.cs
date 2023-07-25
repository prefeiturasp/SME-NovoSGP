using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPResposta : IRepositorioBase<RelatorioPeriodicoPAPResposta>
    {
        Task<IEnumerable<RelatorioPeriodicoPAPResposta>> ObterRespostas(long papSecaoId);
    }
}
