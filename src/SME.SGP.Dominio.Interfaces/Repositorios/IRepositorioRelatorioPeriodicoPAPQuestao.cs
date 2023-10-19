using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPQuestao : IRepositorioBase<RelatorioPeriodicoPAPQuestao>
    {
        Task<IEnumerable<Questao>> ObterQuestoesMigracao();
    }
}
