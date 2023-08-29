using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPSecao : IRepositorioBase<RelatorioPeriodicoPAPSecao>
    {
        Task<RelatorioPeriodicoPAPSecao> ObterSecoesComQuestoes(long id);
    }
}
