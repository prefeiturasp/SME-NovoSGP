using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoAula : IRepositorioBase<CompensacaoAusenciaAlunoAula>
    {
        Task<bool> ExclusaoLogicaCompensacaoAusenciaAlunoAulaPorIds(long[] ids);
    }
}
