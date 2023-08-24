using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPAluno : IRepositorioBase<RelatorioPeriodicoPAPAluno>
    {
        Task<IEnumerable<string>> ObterCodigosDeAlunosComRelatorioJaPreenchido(long turmaId, long periodoRelatorioPAPId);
    }
}
